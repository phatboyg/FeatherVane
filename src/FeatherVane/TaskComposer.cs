// Copyright 2012-2012 Chris Patterson
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
// except in compliance with the License. You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
// ANY KIND, either express or implied. See the License for the specific language governing
// permissions and limitations under the License.
namespace FeatherVane
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;


    public static class TaskComposer
    {
        public static Task Compose<T>(Vane<T> vane, Payload<T> payload, CancellationToken cancellationToken,
            bool runSynchronously = true)
        {
            var composer = new TaskComposer<T>(cancellationToken, runSynchronously);

            vane.Compose(composer, payload);

            return composer.Complete();
        }

        /// <summary>
        /// Compose a source vane into a Task for execution
        /// </summary>
        /// <typeparam name="T">The source vane type</typeparam>
        /// <param name="vane">The source vane</param>
        /// <param name="payload">The payload for the source</param>
        /// <param name="next">The vane after the source</param>
        /// <param name="cancellationToken"></param>
        /// <param name="runSynchronously"></param>
        /// <returns></returns>
        public static Task Compose<T, TPayload>(SourceVane<T> vane, Payload<TPayload> payload, Vane<T> next,
            CancellationToken cancellationToken, bool runSynchronously = true)
        {
            var composer = new TaskComposer<T>(cancellationToken, runSynchronously);

            vane.Compose(composer, payload, next);

            return composer.Complete();
        }

        public static Task Completed<T>(CancellationToken cancellationToken)
        {
            var composer = new TaskComposer<T>(cancellationToken);

            return composer.Complete();
        }
    }


    /// <summary>
    /// Builds a chain of tasks that should run synchronously on the building thread until
    /// an asynchronous operation is requested, in which case it switches the chain to 
    /// asynchronous.
    /// </summary>
    /// <typeparam name="T">The payload type of the task chain</typeparam>
    public class TaskComposer<T> :
        Composer
    {
        readonly CancellationToken _cancellationToken;

        readonly Lazy<Exception> _completeException =
            new Lazy<Exception>(() => new TaskComposerException("The composition is already complete."));

        bool _complete;
        Task _task;

        public TaskComposer(CancellationToken cancellationToken, bool runSynchronously = true)
        {
            _cancellationToken = cancellationToken;
            _task = runSynchronously
                        ? cancellationToken.IsCancellationRequested
                              ? TaskUtil.Cancelled()
                              : TaskUtil.Completed()
                        : Task.Factory.StartNew(() => { }, cancellationToken);
        }

        CancellationToken Composer.CancellationToken
        {
            get { return _cancellationToken; }
        }

        Composer Composer.Execute(Action continuation, bool runSynchronously)
        {
            if (_complete)
                throw _completeException.Value;

            _task = Execute(_task, () => TaskUtil.RunSynchronously(continuation, _cancellationToken), _cancellationToken,
                runSynchronously);
            return this;
        }

        Composer Composer.Execute(Func<Task> continuationTask, bool runSynchronously)
        {
            if (_complete)
                throw _completeException.Value;

            _task = Execute(_task, continuationTask, _cancellationToken, runSynchronously);
            return this;
        }

        Composer Composer.Compensate(Func<Compensation, CompensationResult> compensation)
        {
            if (_complete)
                throw _completeException.Value;

            if (_task.Status == TaskStatus.RanToCompletion)
                return this;

            _task = Compensate(_task, () => compensation(new TaskCompensation<T>(_task)).Task);
            return this;
        }

        void Composer.Completed()
        {
            if (_complete)
                throw _completeException.Value;

            _task = Execute(_task, TaskUtil.Completed, _cancellationToken);
        }

        void Composer.Failed(Exception exception)
        {
            if (_complete)
                throw _completeException.Value;

            _task = Execute(_task, () => TaskUtil.CompletedError(exception), _cancellationToken);
        }

        Composer Composer.Finally(Action continuation, bool runSynchronously)
        {
            if (_complete)
                throw _completeException.Value;

            if (_task.IsCompleted)
            {
                try
                {
                    continuation();
                    return this;
                }
                catch (Exception ex)
                {
                    _task.MarkObserved();
                    _task = TaskUtil.CompletedError(ex);
                    return this;
                }
            }

            _task = FinallyAsync(_task, continuation, runSynchronously);
            return this;
        }

        public Task Complete()
        {
            _complete = true;

            return _task;
        }

        static Task Execute(Task task, Func<Task> continuationTask, CancellationToken cancellationToken,
            bool runSynchronously = true)
        {
            if (task.IsCompleted)
            {
                if (task.IsFaulted)
                    return TaskUtil.CompletedErrors(task.Exception.InnerExceptions);

                if (task.IsCanceled || cancellationToken.IsCancellationRequested)
                    return TaskUtil.Cancelled();

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    try
                    {
                        return continuationTask();
                    }
                    catch (Exception ex)
                    {
                        return TaskUtil.CompletedError(ex);
                    }
                }
            }

            return ExecuteAsync(task, continuationTask, cancellationToken, runSynchronously);
        }

        static Task ExecuteAsync(Task task, Func<Task> continuationTask, CancellationToken cancellationToken,
            bool runSynchronously)
        {
            SynchronizationContext context = SynchronizationContext.Current;

            var source = new TaskCompletionSource<Task>();
            task.ContinueWith(innerTask =>
                {
                    if (innerTask.IsFaulted)
                        source.TrySetException(innerTask.Exception.InnerExceptions);
                    else if (innerTask.IsCanceled || cancellationToken.IsCancellationRequested)
                        source.TrySetCanceled();
                    else
                    {
                        if (context != null)
                        {
                            context.Post(state =>
                                {
                                    try
                                    {
                                        source.TrySetResult(continuationTask());
                                    }
                                    catch (Exception ex)
                                    {
                                        source.TrySetException(ex);
                                    }
                                }, state: null);
                        }
                        else
                            source.TrySetResult(continuationTask());
                    }
                }, runSynchronously
                       ? TaskContinuationOptions.ExecuteSynchronously
                       : TaskContinuationOptions.None);

            return source.Task.FastUnwrap();
        }

        static Task Compensate(Task task, Func<Task> compensationTask)
        {
            if (task.IsCompleted)
            {
                if (task.IsFaulted)
                {
                    try
                    {
                        Task resultTask = compensationTask();
                        if (resultTask == null)
                            throw new InvalidOperationException("Sure could use a Task here buddy");

                        return resultTask;
                    }
                    catch (Exception ex)
                    {
                        return TaskUtil.CompletedError(ex);
                    }
                }

                if (task.IsCanceled)
                    return TaskUtil.Cancelled();

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    var tcs = new TaskCompletionSource<object>();
                    tcs.TrySetFromTask(task);
                    return tcs.Task;
                }
            }

            return CompensateAsync(task, compensationTask);
        }

        static Task CompensateAsync(Task task, Func<Task> compensationTask)
        {
            var source = new TaskCompletionSource<Task>();

            task.ContinueWith(innerTask => source.TrySetFromTask(innerTask),
                TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            SynchronizationContext syncContext = SynchronizationContext.Current;

            task.ContinueWith(innerTask =>
                {
                    if (syncContext != null)
                    {
                        syncContext.Post(state =>
                            {
                                try
                                {
                                    Task resultTask = compensationTask();
                                    if (resultTask == null)
                                        throw new InvalidOperationException("Sure could use a Task here buddy");

                                    source.TrySetResult(resultTask);
                                }
                                catch (Exception ex)
                                {
                                    source.TrySetException(ex);
                                }
                            }, state: null);
                    }
                    else
                    {
                        try
                        {
                            Task resultTask = compensationTask();
                            if (resultTask == null)
                                throw new InvalidOperationException("Sure could use a Task here buddy");

                            source.TrySetResult(resultTask);
                        }
                        catch (Exception ex)
                        {
                            source.TrySetException(ex);
                        }
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);

            return source.Task.FastUnwrap();
        }


        static Task FinallyAsync(Task task, Action continuation, bool runSynchronously = true)
        {
            SynchronizationContext syncContext = SynchronizationContext.Current;

            var source = new TaskCompletionSource<object>();
            task.ContinueWith(innerTask =>
                {
                    try
                    {
                        if (syncContext != null)
                        {
                            syncContext.Post(state =>
                                {
                                    try
                                    {
                                        continuation();
                                        source.TrySetFromTask(innerTask);
                                    }
                                    catch (Exception ex)
                                    {
                                        innerTask.MarkObserved();
                                        source.SetException(ex);
                                    }
                                }, state: null);
                        }
                        else
                        {
                            continuation();
                            source.TrySetFromTask(innerTask);
                        }
                    }
                    catch (Exception ex)
                    {
                        innerTask.MarkObserved();
                        source.TrySetException(ex);
                    }
                }, runSynchronously
                       ? TaskContinuationOptions.ExecuteSynchronously
                       : TaskContinuationOptions.None);

            return source.Task;
        }
    }
}