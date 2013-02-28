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
namespace FeatherVane.Support.CircuitBreakerFeather
{
    using System;
    using System.Collections.Generic;
    using System.Threading;


    /// <summary>
    /// Represents a circuit that is unavailable, with a timer waiting to partially close
    /// the circuit.
    /// </summary>
    class OpenCircuitBreakerState :
        CircuitBreakerState
    {
        readonly Exception _exception;
        readonly IEnumerator<int> _timeoutEnumerator;
        readonly Timer _timer;

        public OpenCircuitBreakerState(CircuitBreaker breaker, Exception exception, IEnumerator<int> timeoutEnumerator)
            : base(breaker)
        {
            _exception = exception;
            _timeoutEnumerator = timeoutEnumerator;

            _timer = GetTimer(timeoutEnumerator);
        }

        public override void BeforeExecute()
        {
            throw new CircuitOpenException("The circuit breaker is open", _exception);
        }

        Timer GetTimer(IEnumerator<int> timeoutEnumerator)
        {
            timeoutEnumerator.MoveNext();

            return new Timer(PartiallyCloseCircuit, this, timeoutEnumerator.Current, -1);
        }

        void PartiallyCloseCircuit(object state)
        {
            _timer.Dispose();
            Breaker.ClosePartially(_exception, _timeoutEnumerator);
        }
    }
}