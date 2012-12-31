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
namespace FeatherVane.FeatherVaneConfigurators
{
    using System.Collections.Generic;
    using System.Linq;
    using Configurators;
    using FeatherVaneBuilders;
    using VaneBuilders;


    public class FanoutConfiguratorImpl<T> :
        FanoutConfigurator<T>,
        VaneBuilderConfigurator<T>
    {
        readonly IList<VaneBuilderConfigurator<T>> _vaneBuilderConfigurators;

        public FanoutConfiguratorImpl()
        {
            _vaneBuilderConfigurators = new List<VaneBuilderConfigurator<T>>();
        }

        void VaneConfigurator<T>.Add(VaneBuilderConfigurator<T> vaneBuilderConfigurator)
        {
            _vaneBuilderConfigurators.Add(vaneBuilderConfigurator);
        }

        void VaneBuilderConfigurator<T>.Configure(VaneBuilder<T> builder)
        {
            var fanoutBuilder = new FanoutBuilder<T>();

            foreach (var configurator in _vaneBuilderConfigurators)
                configurator.Configure(fanoutBuilder);

            builder.Add(fanoutBuilder);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            return _vaneBuilderConfigurators.SelectMany(vaneConfigurator => vaneConfigurator.Validate());
        }
    }
}