# FeatherVane

## Introduction

FeatherVane is a lightweight and extensible middleware framework for building compositional .NET applications. Built on top of the Task Parallel Library (or TPL), FeatherVane leverages the power of _Tasks_, enabling the creation of decoupled components called FeatherVanes that can both execute quickly and remain "-Async" friendly. FeatherVane is middleware, and is designed for use between infrastructure and application code. Infrastructure includes services such as web servers, message queues, and resources such as files and sockets. Using Tasks allows FeatherVane to support asynchronous execution at any point in a vane, allowing the creation of high performance applications that take full advantage of all available processing cores. By leveraging the advanced features of the TPL, synchronous vanes run at full speed without incurring the cost of background execution while still supporting asynchronous operations at any point in the vane.

> In .NET 4.0, Microsoft introduced the Task Parallel Library (TPL) providing a new programming model for building asynchronous applications. In this model, a _Task_ represents a unit of work that executes on the _ThreadPool_. Since FeatherVane makes extensive use of the TPL, the framework can only be used with applications targeting .NET 4.0 (or later, including .NET 4.5 and the .NET Core 4.5 version used by Windows Store applications).


## The Atoms

There are several atoms within FeatherVane that can be composed together to provide the functionality required by the application. Each of these atoms plays a specific role, as described in the following sections.


### The FeatherVane Atom

A _FeatherVane_ is a middleware component that performs a specific function. Since a middleware layer is often composed of multiple components, a FeatherVane should adhere to the [ single responsibility principle ](http://en.wikipedia.org/wiki/Single_responsibility_principle) -- do one thing and one thing only. By creating fine grained FeatherVanes, developers are able to opt-in to each component without including unnecessary or unwanted functionality. There are many FeatherVanes included in the framework, providing functionality such as logging, profiling, and exception handling. There are also several namespaces containing advanced FeatherVanes, such as messaging, routing, and even the ability to splice objects loaded from a database into the vane.

To create a FeatherVane, a developer creates a class that implements the _FeatherVane<T>_ interface. The interface has a single method, _Compose_, which is shown below.

    public interface FeatherVane<T>
    {
        void Compose(Composer composer, Payload<T> payload, Vane<T> next);
    }


Middleware provides the glue between infrastructure and application code, so it is important that any type of payload can be transfered from the infrastructure to the application. By leveraging generic types, _FeatherVane<T>_ is a generic interface, FeatherVanes can be written as an open generic type and support any payload type as well as a closed generic type designed for a specific payload type. There are no generic constraints, allowing reference and value types to be transferred efficiently without boxing.

In the FeatherVane<T> interface, the _Compose_ method is called to compose a _Task_ for execution. The _composer_ is used to add behavior to the Task, and includes executing code, compensating for exceptions, and cleaning up after execution. The _payload_ wraps the generic type of the FeatherVane and contains additional context that is transferred out of band as well as allowing additional payload types to be created without breaking the connection to the original payload. 

The final argument, _next_, references the next _Vane_ to be composed. An individual FeatherVane has no concept of next until it is composed into a _Vane_, at which point the linkage between vanes is realized. The link to the _next_ vane is passed into the _Compose_ method by way of the _next_ argument, allowing a FeatherVane to pass control to the next FeatherVane so that it can compose itself as well. This continues until a terminal vane is encountered. The composition of FeatherVanes into a Vane is done in a way that gives each FeatherVane complete control over how the payload is transferred to the next FeatherVane. The FeatherVane can compose operations in any order desired, including the point at which the next FeatherVane is composed.

> A _terminal vane_ is a _Vane_ that does not point to another FeatherVane, but instead returns immediately.


### The Vane Atom

A set of individual FeatherVanes are composed into a _Vane_. The Vane is then executed by a payload source (usually an infrastructure component, but a vane may also be executed by a different vane as vanes can be composed into larger vanes). Once a vane has been composed, the vane is referenced using a single interface method, which is shown below.

    public interface Vane<T>
    {
        void Compose(Composer composer, Payload<T> payload);
    }



To execute a vane with a payload, there are many extension methods on the _Vane_ interface. There are synchronous and asynchronous _Execute_ methods available, each of which are shown below.

    int SomeInfrastructureHandler(InputModel model)
    {
        // invoke the vane synchronously
        _vane.Execute(model);
    }
    
    Task SomeInfrastructureHandlerAsync(InputModel model, CancellationToken cancellationToken)
    {
        return _vane.ExecuteAsync(model, cancellationToken);
    }
    
    
In the first example, the _model_ is passed to the _Execute_ method on the _Vane_. The extension method creates a payload wrapper (of the type _Payload<InputModel>_) which matches the vane type (which would be _Vane<InputModel>_). The extension method then creates a _TaskComposer_ and uses it to _compose_ the _Task_. In the case of the synchronous version, the _Task_ is examined to see if it has already completed, and if it is not finished, the _Wait_ method is called. By default, the timeout is infinite, but a timeout can be specified using a _CancellationToken_ (a helper function is available to create the timeout-based cancellationToken as well).

### The SourceVane Atom

A SourceVane provides an additional type that can be spliced into the Vane, resulting in an output tuple that is composed into the output vane of the splice.


## Task Composition

In the _Compose_ method, the _Composer_ is used to add behavior to the _Task_ that executed. For example, if a FeatherVane is created to write a string payload to the console, the compose method would be written as shown below.

    void Compose(Composer composer, Payload<string> payload, Vane<string> next)
    {
        composer.Execute(() => Console.WriteLine("Payload: {0}", payload.Data);
        
        next.Compose(composer, payload);
    }


The _Execute_ method on the composer adds a method (whether it is a delegate, lambda method, class method, etc.) that is called as part of the execution of the Vane. There are multiple ways in which methods can be added to an execution, but it is important that any processing be added using the composer, and not within the _Compose_ method itself. The reason is that previously composed FeatherVanes __may__ not have completed, resulting an operations being executed on payloads that are not ready yet.

### Synchronous versus Asynchronous Execution

By default, FeatherVane executes synchronously on the thread that called the _Execute_ method. The reason for this is performance; if there are no asynchronous operations performed within a Vane, executing the vane asynchronously results in significantly longer execution time. By keeping synchronous operations on the same thread, vanes execute and complete quickly.

This includes the _ExecuteAsync_ method, as often a vane needs to perform a series of synchronous operations before reaching an asynchronous operation (such as reading a file, retrieving a web page via HTTP, etc.). 

To force a vane to execute asynchronously, the optional argument _runSynchronously_ can be set to true. This forces the entire execution to occur on a background thread which is scheduled and managed entirely by the TPL. The only work performed on the actual calling thread is the initial composition of the vane, which is minimal and depending upon the contents of the vane may immediately defer to another thread as well.

#### How is this possible?

In the Task Parallel Library, there are multiple ways to create and execute a _Task_. In most usage scenarios, the _Task.Run_ method is used, passing the appropriate method which is then scheduled and executed on a thread pool thread. While this is often the best way to run code in the background, particularly in the situation where the code is being run from the UI thread, there are more advanced approaches that can yield faster performance (pun intended). When code can be executed synchronously, the code is executed and a _TaskCompletionSource_ is used to retain the Task semantics used by the _TaskComposer_. The details of how synchronous operations are composed without incurring the expense of a thread switch are all handled by the TaskComposer, leaving the developer to simply specify methods to be executed.

#### What about asynchronous methods?

Many methods that perform IO, such as calling a web server or reading from a file, are available in an asynchronous form. Often they are separated from other methods by an _Async_ suffix, or a _Begin_/_End_ prefix. These method initiate an asynchronous operation, and immediately return to the caller. The caller can then continue with other operations until the result of the asynchronous operation is needed. If the asynchronous operation has completed, the results of the operation are returned and execution continues. 

However, if the asynchronous operation has not yet completed, the caller must _wait_ until it completes. Too often, this waiting consists of _blocking_ the current thread which prevents the thread from being used by other operations. This causes the thread to sit idle consuming resources that are better used to perform other operations.

To avoid blocking threads, FeatherVane allows the Task from the asynchronous operation to be used by the composer. An overload of the _Execute_ method allows the method to return a _Task_ (instead of the void return method of the _Action_ version), which is then used by the composer when composing subsequent FeatherVanes. An example of returning as Task is shown below.

    void Compose(Composer composer, Payload<Uri> payload, Vane<Uri> next)
    {
        composer.Execute(() =>
            {
                var task = _httpClient.GetAsync(payload.Data);
                var returnTask = task.ContinueWith(messageTask =>
                    {
                        // do something with the HttpMessage
                    });
                    
                return returnTask;
            });
            
        next.Compose(composer, payload);
    }
    
    
In the example above, the FeatherVane uses _HttpClient_ to request the content at the _Uri_ specified in the payload. A continuation is then added to the _Task_ returned, allowing the result of the request to be processed. The task returned by _ContinueWith_ is then returned by the lambda method, which _Execute_ uses to continue composing the remaining FeatherVanes. Since _next_ is composed after the _Execute_, any operations added by the next vane will be executed after the asynchronous operations are completed.

> By building FeatherVane on top of the TPL, the compositional nature of the TPL is retained. This makes FeatherVane an incredibly flexible middleware layer, as flexible as the TPL itself but with some additional capabilities (as well as a few constraints).

### Exceptions

It is true, bad things can sometimes happen, and how a middleware layer reacts to exceptions is important.
