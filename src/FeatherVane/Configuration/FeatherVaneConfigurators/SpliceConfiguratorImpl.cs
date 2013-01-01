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
    using VaneBuilders;
    using VaneConfigurators;
    using Vanes;


    public class SpliceConfiguratorImpl<T, TSource> :
        SpliceConfigurator<T, TSource>,
        VaneBuilderConfigurator<T>
    {
        readonly SourceVaneFactory<TSource> _sourceVaneFactory;
        readonly VaneConfiguratorImpl<Tuple<T, TSource>> _vaneConfigurator;

        public SpliceConfiguratorImpl(SourceVaneFactory<TSource> sourceVaneFactory)
        {
            _sourceVaneFactory = sourceVaneFactory;
            _vaneConfigurator = new SuccessConfigurator<Tuple<T, TSource>>();
        }

        void VaneConfigurator<Tuple<T, TSource>>.Add(VaneBuilderConfigurator<Tuple<T, TSource>> vaneBuilderConfigurator)
        {
            _vaneConfigurator.Add(vaneBuilderConfigurator);
        }

        void VaneBuilderConfigurator<T>.Configure(VaneBuilder<T> builder)
        {
            Vane<Tuple<T, TSource>> outputVane = _vaneConfigurator.Create();
            SourceVane<TSource> sourceVane = _sourceVaneFactory.Create();

            var splice = new Splice<T, TSource>(outputVane, sourceVane);

            builder.Add(splice);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            Configurator configurator = _vaneConfigurator;

            return configurator.Validate();
        }
    }
}