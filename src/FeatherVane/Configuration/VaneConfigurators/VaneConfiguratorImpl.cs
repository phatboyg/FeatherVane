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
namespace FeatherVane.VaneConfigurators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configurators;
    using VaneBuilders;


    public class VaneConfiguratorImpl<T> :
        VaneConfigurator<T>,
        VaneFactory<T>
    {
        readonly Func<Vane<T>> _tailFactory;
        readonly IList<VaneBuilderConfigurator<T>> _configurators;

        public VaneConfiguratorImpl(Func<Vane<T>> tailFactory)
        {
            _tailFactory = tailFactory;

            _configurators = new List<VaneBuilderConfigurator<T>>();
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            if (_tailFactory == null)
                yield return this.Failure("TailFactory", "must not be null");

            foreach (ValidateResult result in _configurators.SelectMany(x => x.Validate()))
                yield return result;
        }

        Vane<T> VaneFactory<T>.Create()
        {
            var builder = new VaneBuilderImpl<T>(_tailFactory);

            foreach (var configurator in _configurators)
                configurator.Configure(builder);

            return builder.Build();
        }

        public void Add(VaneBuilderConfigurator<T> vaneBuilderConfigurator)
        {
            _configurators.Add(vaneBuilderConfigurator);
        }
    }
}