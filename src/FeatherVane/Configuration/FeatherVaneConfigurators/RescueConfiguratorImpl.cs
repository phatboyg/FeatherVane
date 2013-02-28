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
    using System.Linq;
    using Configurators;
    using Feathers;
    using VaneBuilders;
    using Vanes;


    public class RescueConfiguratorImpl<T> :
        RescueConfigurator<T>,
        VaneBuilderConfigurator<T>
    {
        readonly IList<VaneBuilderConfigurator<T>> _configurators;
        Func<Vane<T>> _tailFactory;

        public RescueConfiguratorImpl()
            : this(() => new SuccessVane<T>())
        {
        }

        public RescueConfiguratorImpl(Func<Vane<T>> tailFactory)
        {
            _tailFactory = tailFactory;
            _configurators = new List<VaneBuilderConfigurator<T>>();
        }

        void VaneConfigurator<T>.Add(VaneBuilderConfigurator<T> vaneBuilderConfigurator)
        {
            _configurators.Add(vaneBuilderConfigurator);
        }

        void VaneBuilderConfigurator<T>.Configure(VaneBuilder<T> builder)
        {
            Vane<T> rescueVane = BuildRescueVane();

            var vane = new RescueFeather<T>(rescueVane);
            builder.Add(vane);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            return _configurators.SelectMany(x => x.Validate());
        }

        Vane<T> BuildRescueVane()
        {
            var rescueBuilder = new VaneBuilderImpl<T>(_tailFactory);

            foreach (var configurator in _configurators)
                configurator.Configure(rescueBuilder);

            Vane<T> rescue = rescueBuilder.Build();
            return rescue;
        }
    }
}