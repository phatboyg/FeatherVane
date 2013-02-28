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
namespace FeatherVane.FeatherVaneConfigurators
{
    using System;
    using System.Collections.Generic;
    using Configurators;
    using Feathers;
    using VaneBuilders;


    public class ExecuteConfigurator<T> :
        FeatherConfigurator<T>,
        VaneBuilderConfigurator<T>
    {
        readonly Action<Payload<T>> _continuation;

        public ExecuteConfigurator(Action<Payload<T>> continuation)
        {
            _continuation = continuation;
        }

        void VaneBuilderConfigurator<T>.Configure(VaneBuilder<T> builder)
        {
            var execute = new ExecuteFeather<T>(_continuation);
            builder.Add(execute);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            if (_continuation == null)
                yield return this.Failure("Continuation", "must not be null");
        }
    }
}