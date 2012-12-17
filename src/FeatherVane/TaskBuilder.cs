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


    /// <summary>
    /// Builds a chain of tasks that should run synchronously on the building thread until
    /// an asynchronous operation is requested, in which case it switches the chain to 
    /// asynchronous.
    /// </summary>
    /// <typeparam name="T">The payload type of the task chain</typeparam>
    public class TaskBuilder<T> :
        Builder<T>
    {
        bool _built;
        CancellationToken _cancellationToken;
        Task _task;

        public TaskBuilder(CancellationToken cancellationToken = default(CancellationToken))
        {
            _task = TaskUtil.Completed();
            _cancellationToken = cancellationToken;
        }

        CancellationToken Builder<T>.CancellationToken
        {
            get { return _cancellationToken; }
        }

        Builder<T> Builder<T>.Execute(Action continuation, bool runSynchronously)
        {
            if (_built)
                throw new TaskBuilderException("The plan has already been built.");

            Then(() => TaskUtil.RunSynchronously(continuation, _cancellationToken), runSynchronously);
            return this;
        }

        Builder<T> Builder<T>.Execute(Func<Task> continuationTask, bool runSynchronously)
        {
            if (_built)
                throw new TaskBuilderException("The plan has already been built.");

            Then(continuationTask, runSynchronously);
            return this;
        }

        Builder<T> Builder<T>.Compensate(Func<Compensation, CompensationResult> compensation)
        {
            if (_built)
                throw new TaskBuilderException("The plan has already been built.");

            if (_task.Status == TaskStatus.RanToCompletion)
                return this;

            _task = Compensate(_task, () => compensation(new CompensationImpl(_task)).Task);
            return this;
        }

        void Builder<T>.Completed()
        {
            if (_built)
                throw new TaskBuilderException("The plan has already been built.");

            Then(TaskUtil.Completed);
        }

        void Builder<T>.Failed(Exception exception)
        {
            if (_built)
                throw new TaskBuilderException("The plan has already been built.");

            Then(() => TaskUtil.CompletedError(exception));
        }

        Builder<T> Builder<T>.Finally(Action continuation, bool runSynchronously = true)
        {
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

            FinallyAsync(continuation, runSynchronously);
            return this;
        }

        public Task Build()
        {
            _built = true;

            return _task;
        }

        void Then(Func<Task> newTask, bool runSynchronously = true)
        {
            if (_task.IsCompleted)
            {
                if (_task.IsFaulted)
                {
                    _task = TaskUtil.CompletedErrors(_task.Exception.InnerExceptions);
                    return;
                }
                if (_task.IsCanceled || _cancellationToken.IsCancellationRequested)
                {
                    _task = TaskUtil.Cancelled();
                    return;
                }
                if (_task.Status == TaskStatus.RanToCompletion)
                {
                    try
                    {
                        _task = newTask();
                        return;
                    }
                    catch (Exception ex)
                    {
                        _task = TaskUtil.CompletedError(ex);
                        return;
                    }
                }
            }

            _task = ThenAsync(newTask, runSynchronously);
        }


        Task ThenAsync(Func<Task> newTask, bool runSynchronously)
        {
            SynchronizationContext context = SynchronizationContext.Current;

            var source = new TaskCompletionSource<Task>();
            _task.ContinueWith(task =>
                {
                    if (task.IsFaulted)
                        source.TrySetException(task.Exception.InnerExceptions);
                    else if (task.IsCanceled || _cancellationToken.IsCancellationRequested)
                        source.TrySetCanceled();
                    else
                    {
                        if (context != null)
                        {
                            context.Post(state =>
                                {
                                    try
                                    {
                                        source.TrySetResult(newTask());
                                    }
                                    catch (Exception ex)
                                    {
                                        source.TrySetException(ex);
                                    }
                                }, state: null);
                        }
                        else
                            source.TrySetResult(newTask());
                    }
                }, runSynchronously
                       ? TaskContinuationOptions.ExecuteSynchronously
                       : TaskContinuationOptions.None);

            return source.Task.FastUnwrap();
        }

        Task Compensate(Task task, Func<Task> continuation)
        {
            if (task.IsCompleted)
            {
                if (task.IsFaulted)
                {
                    try
                    {
                        Task resultTask = continuation();
                        if (resultTask == null)
                            throw new InvalidOperationException("Sure could use a Task here buddy");

                        return resultTask;
                    }
                    catch (Exception ex)
                    {
                        return TaskUtil.CompletedError(ex);
                    }
                }

                if (task.IsCanceled || _cancellationToken.IsCancellationRequested)
                    return TaskUtil.Cancelled();

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    var tcs = new TaskCompletionSource<object>();
                    tcs.TrySetFromTask(task);
                    return tcs.Task;
                }
            }

            return CompensateAsync(task, continuation);
        }

        static Task CompensateAsync(Task task, Func<Task> continuation)
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
                                    Task resultTask = continuation();
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
                            Task resultTask = continuation();
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


        void FinallyAsync(Action continuation, bool runSynchronously = true)
        {
            SynchronizationContext syncContext = SynchronizationContext.Current;

            var source = new TaskCompletionSource<object>();
            _task.ContinueWith(innerTask =>
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

            _task = source.Task;
        }
    }
}