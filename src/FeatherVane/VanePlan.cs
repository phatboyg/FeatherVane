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
    using System.Collections.Generic;

    public class VanePlan<T> :
        Plan<T>
        where T : class
    {
        readonly Payload<T> _payload;
        IList<Exception> _exceptions;
        int _nextStep;
        Step<T>[] _steps;

        public VanePlan(Step<T>[] steps, Payload<T> payload)
        {
            _steps = steps;
            _payload = payload;
            
            _exceptions = new List<Exception>();
        }

        public bool IsCompleted
        {
            get { return _nextStep >= _steps.Length; }
        }

        public bool IsExecuting
        {
            get { return _nextStep > 0; }
        }

        public bool Execute()
        {
            if (IsCompleted)
                return true;

            Step<T> step = _steps[_nextStep++];
            bool ok = false;
            try
            {
                ok = step.Execute(this);
            }
            catch (Exception ex)
            {
                _exceptions.Add(ex);
            }

            return ok || Compensate();
        }

        public bool Compensate()
        {
            if (!IsExecuting)
                return false;

            Step<T> step = _steps[--_nextStep];
  
            // TODO need to capture this stuff
            return step.Compensate(this);
        }

        public int Length
        {
            get { return _steps.Length; }
        }

        public Payload<T> Payload
        {
            get { return _payload; }
        }
    }
}