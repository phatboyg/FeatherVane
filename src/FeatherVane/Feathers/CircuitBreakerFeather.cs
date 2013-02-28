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
namespace FeatherVane.Feathers
{
    using System;
    using System.Collections.Generic;
    using Support.CircuitBreakerFeather;


    /// <summary>
    /// Prevents a vane from being overloaded when it is unavailable due to a reoccurring 
    /// exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CircuitBreakerFeather<T> :
        Feather<T>,
        CircuitBreaker
    {
        readonly int _closeThreshold;
        readonly object _lock = new object();
        readonly int _openThreshold;
        CircuitBreakerState _state;

        public CircuitBreakerFeather(int openThreshold, int closeThreshold)
        {
            _openThreshold = openThreshold;
            _closeThreshold = closeThreshold;

            Close();
        }

        static IEnumerable<int> Timeouts
        {
            get
            {
                yield return 100;
                yield return 1000;
                yield return 2000;
                yield return 5000;
                yield return 10000;
                while (true)
                    yield return 30000;
            }
        }

        public int OpenThreshold
        {
            get { return _openThreshold; }
        }

        public int CloseThreshold
        {
            get { return _closeThreshold; }
        }

        public void Open(Exception exception, IEnumerator<int> timeoutEnumerator = null)
        {
            if (timeoutEnumerator == null)
                timeoutEnumerator = Timeouts.GetEnumerator();

            lock (_lock)
                _state = new OpenCircuitBreakerState(this, exception, timeoutEnumerator);
        }

        public void Close()
        {
            lock (_lock)
                _state = new ClosedCircuitBreakerState(this);
        }

        public void ClosePartially(Exception exception, IEnumerator<int> timeoutEnumerator)
        {
            lock (_lock)
                _state = new PartiallyClosedCircuitBreakerState(this, exception, timeoutEnumerator);
        }

        public void Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            composer.Execute(() =>
                {
                    lock (_lock)
                        _state.BeforeExecute();

                    return composer.ComposeTask(next, payload);
                });

            composer.Execute(() =>
                {
                    lock (_state)
                        _state.ExecuteCompleted();
                });

            composer.Compensate(compensation =>
                {
                    lock (_lock)
                        _state.ExecuteFaulted(compensation.Exception);

                    return compensation.Throw();
                });
        }
    }
}