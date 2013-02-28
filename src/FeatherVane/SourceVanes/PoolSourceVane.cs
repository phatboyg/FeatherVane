// Copyright 2012-2013 Chris Patterson
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
namespace FeatherVane.SourceVanes
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;


    public class PoolSourceVane<T> :
        SourceVane<T>,
        IDisposable
    {
        readonly Stack<T> _available;
        readonly SourceVane<T> _createVane;
        readonly Vane<T> _destroyVane;
        readonly HashSet<T> _inUse;
        readonly object _lock;

        public PoolSourceVane(SourceVane<T> createVane, Vane<T> destroyVane)
        {
            _createVane = createVane;
            _destroyVane = destroyVane;

            _available = new Stack<T>();
            _inUse = new HashSet<T>();
            _lock = new object();
        }

        public void Dispose()
        {
            lock (_lock)
            {
                DateTime timeout = DateTime.Now + TimeSpan.FromSeconds(30);
                while (_inUse.Count > 0 && timeout > DateTime.Now)
                {
                    Monitor.PulseAll(_lock);
                    Thread.SpinWait(100);
                }

                while (_available.Count > 0)
                {
                    T instance = _available.Pop();
                    _destroyVane.Execute(instance);
                }
            }
        }

        public void Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, T>> next)
        {
            composer.Execute(() =>
                {
                    T instance = default(T);
                    bool useExisting = false;
                    lock (_lock)
                    {
                        if (_available.Count > 0)
                        {
                            instance = _available.Pop();
                            _inUse.Add(instance);
                            useExisting = true;
                        }
                    }

                    var track = new Track<TPayload>(this, next);

                    if (useExisting)
                        return composer.ComposeTask(track, payload.MergeRight(instance));

                    return composer.ComposeTask(_createVane, payload, track);
                });
        }

        void Retain(T instance)
        {
            lock (_lock)
            {
                _inUse.Remove(instance);
                _available.Push(instance);
            }
        }

        Task Release(T instance)
        {
            lock (_lock)
                _inUse.Remove(instance);

            return _destroyVane.ExecuteAsync(instance);
        }


        class Track<TPayload> :
            Vane<Tuple<TPayload, T>>
        {
            readonly Vane<Tuple<TPayload, T>> _next;
            readonly PoolSourceVane<T> _poolSourceVane;

            public Track(PoolSourceVane<T> poolSourceVane, Vane<Tuple<TPayload, T>> next)
            {
                _poolSourceVane = poolSourceVane;
                _next = next;
            }

            public void Compose(Composer composer, Payload<Tuple<TPayload, T>> payload)
            {
                _next.Compose(composer, payload);

                composer.Execute(() => _poolSourceVane.Retain(payload.Right()));

                composer.Compensate(compensation => compensation.Task(composer.ComposeTask(payload, (x, p) =>
                    {
                        x.Execute(() => _poolSourceVane.Release(p.Right()));

                        x.Failed(compensation.Exception);
                    })));
            }
        }
    }
}