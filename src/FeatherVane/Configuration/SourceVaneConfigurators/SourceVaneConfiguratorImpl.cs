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
namespace FeatherVane.SourceVaneConfigurators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configurators;
    using Factories;
    using SourceVaneBuilders;


    public class SourceVaneConfiguratorImpl<T> :
        SourceVaneFactory<T>,
        SourceVaneConfigurator<T>
    {
        readonly IList<VaneBuilderConfigurator<T>> _configurators;
        Lazy<SourceVane<T>> _sourceVane;
        Func<SourceVane<T>> _sourceVaneFactory;

        public SourceVaneConfiguratorImpl()
        {
            _configurators = new List<VaneBuilderConfigurator<T>>();
            _sourceVane = new Lazy<SourceVane<T>>(Create);
        }

        void VaneConfigurator<T>.Add(VaneBuilderConfigurator<T> vaneBuilderConfigurator)
        {
            _configurators.Add(vaneBuilderConfigurator);
        }

        void SourceVaneConfigurator<T>.UseSourceVane(Func<SourceVane<T>> sourceVaneFactory)
        {
            _sourceVaneFactory = sourceVaneFactory;
        }

        SourceVane<T> SourceVaneFactory<T>.Create()
        {
            return _sourceVane.Value;
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            if (_sourceVaneFactory == null)
                yield return this.Failure("SourceVaneFactory", "must not be null");

            foreach (ValidateResult result in _configurators.SelectMany(x => x.Validate()))
                yield return result;
        }

        SourceVane<T> Create()
        {
            var builder = new SourceVaneBuilder<T>(_sourceVaneFactory);
            foreach (var configurator in _configurators)
                configurator.Configure(builder);

            return builder.Build();
        }
    }
}