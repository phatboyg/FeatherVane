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
namespace FeatherVane.RabbitMQIntegration.ConnectionManagement
{
    using System;
    using System.Collections.Generic;
    using System.Threading;


    public class BlockingObjectPool<T> :
        ObjectPool<T>
        where T : class
    {
        readonly Stack<T> _available;
        readonly HashSet<T> _inUse;
        readonly object _lock;
        readonly PooledObjectFactory<T> _objectFactory;

        public BlockingObjectPool(PooledObjectFactory<T> objectFactory)
        {
            _objectFactory = objectFactory;

            _available = new Stack<T>();
            _inUse = new HashSet<T>();
            _lock = new object();
        }

        public T Acquire()
        {
            lock (_lock)
            {
                T instance = null;
                if (_available.Count > 0)
                {
                    instance = _available.Pop();
                    if (!_objectFactory.Validate(instance))
                    {
                        _objectFactory.Dispose(instance);
                        instance = null;
                    }
                }

                if (instance == null)
                    instance = _objectFactory.New();

                _inUse.Add(instance);
                return instance;
            }
        }

        public void Surrender(T instance, bool dispose)
        {
            lock (_lock)
            {
                if (!_inUse.Contains(instance))
                    throw new ArgumentException("Object reference not tracked", "instance");

                _inUse.Remove(instance);

                if (dispose || !_objectFactory.Validate(instance))
                    _objectFactory.Dispose(instance);
                else
                    _available.Push(instance);
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                DateTime timeout = DateTime.Now + TimeSpan.FromSeconds(30);
                while (_inUse.Count > 0 && timeout > DateTime.Now)
                {
                    Monitor.Pulse(_lock);
                    Thread.SpinWait(100);
                }

                while (_available.Count > 0)
                {
                    T instance = _available.Peek();
                    _objectFactory.Dispose(instance);
                }
            }
        }
    }
}