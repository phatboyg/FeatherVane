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
    using SourceVaneConfigurators;
    using VaneBuilders;
    using VaneConfigurators;
    using Vanes;


    public class SpliceConfiguratorImpl<T, TSource> :
        SpliceConfigurator<T, TSource>,
        SourceVaneConfigurator<TSource>,
        VaneBuilderConfigurator<T>
    {
        readonly VaneConfiguratorImpl<Tuple<T, TSource>> _vaneConfigurator;
        readonly SourceVaneConfiguratorImpl<TSource> _sourceConfigurator;

        public SpliceConfiguratorImpl()
        {
            _vaneConfigurator = new SuccessConfigurator<Tuple<T, TSource>>();
            _sourceConfigurator = new SourceVaneConfiguratorImpl<TSource>();
        }

        void SourceVaneConfigurator<TSource>.UseSourceVane(Func<SourceVane<TSource>> sourceVaneFactory)
        {
            ((SourceVaneConfigurator<TSource>)_sourceConfigurator).UseSourceVane(sourceVaneFactory);
        }

        void VaneConfigurator<TSource>.Add(VaneBuilderConfigurator<TSource> vaneBuilderConfigurator)
        {
            ((SourceVaneConfigurator<TSource>)_sourceConfigurator).Add(vaneBuilderConfigurator);
        }

        void VaneConfigurator<Tuple<T, TSource>>.Add(VaneBuilderConfigurator<Tuple<T, TSource>> vaneBuilderConfigurator)
        {
            _vaneConfigurator.Add(vaneBuilderConfigurator);
        }

        void VaneBuilderConfigurator<T>.Configure(VaneBuilder<T> builder)
        {
            Vane<Tuple<T, TSource>> outputVane = _vaneConfigurator.Create();
            SourceVane<TSource> sourceVane = _sourceConfigurator.Create();

            var splice = new SpliceVane<T, TSource>(outputVane, sourceVane);

            builder.Add(splice);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            Configurator vaneConfigurator = _vaneConfigurator;
            Configurator sourceConfigurator = _sourceConfigurator;

            return vaneConfigurator.Validate().Concat(sourceConfigurator.Validate());
        }
    }
}