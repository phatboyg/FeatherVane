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
namespace FeatherVane.Vanes.CircuitBreakerSupport
{
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// Executes until the success count is met. If a fault occurs before the success 
    /// count is reached, the circuit reopens.
    /// </summary>
    class PartiallyClosedCircuitBreakerState :
        CircuitBreakerState
    {
        readonly Exception _exception;
        readonly object _lock = new object();
        readonly IEnumerator<int> _timeoutEnumerator;
        int _successCount;

        public PartiallyClosedCircuitBreakerState(CircuitBreaker breaker, Exception exception,
            IEnumerator<int> timeoutEnumerator)
            : base(breaker)
        {
            _exception = exception;
            _timeoutEnumerator = timeoutEnumerator;
        }

        public override void ExecuteCompleted()
        {
            lock (_lock)
            {
                _successCount++;
                if (_successCount >= Breaker.CloseThreshold)
                {
                    Breaker.Close();
                    _timeoutEnumerator.Dispose();
                }
            }
        }

        public override void ExecuteFaulted(Exception exception)
        {
            Breaker.Open(_exception, _timeoutEnumerator);
        }
    }
}