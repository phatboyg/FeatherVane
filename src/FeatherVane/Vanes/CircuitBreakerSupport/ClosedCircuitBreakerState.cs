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


    /// <summary>
    /// Represents a closed, normally operating circuit breaker state
    /// </summary>
    class ClosedCircuitBreakerState :
        CircuitBreakerState
    {
        readonly object _lock = new object();
        int _failureCount;

        public ClosedCircuitBreakerState(CircuitBreaker breaker)
            : base(breaker)
        {
        }

        public override void ExecuteFaulted(Exception exception)
        {
            lock (_lock)
            {
                _failureCount++;
                if (_failureCount >= Breaker.OpenThreshold)
                    Breaker.Open(exception);
            }
        }
    }
}