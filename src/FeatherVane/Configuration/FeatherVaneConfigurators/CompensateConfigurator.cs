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


    public class CompensateConfigurator<T> :
        FeatherConfigurator<T>,
        VaneBuilderConfigurator<T>
    {
        readonly Func<Payload<T>, bool> _compensate;

        public CompensateConfigurator(Func<Payload<T>, bool> compensate)
        {
            _compensate = compensate;
        }

        void VaneBuilderConfigurator<T>.Configure(VaneBuilder<T> builder)
        {
            var execute = new CompensateFeather<T>(_compensate);
            builder.Add(execute);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            if (_compensate == null)
                yield return this.Failure("Compensate", "must not be null");
        }
    }
}