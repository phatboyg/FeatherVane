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


    public abstract class VaneConfiguratorImpl<T> :
        Configurator
    {
        readonly IList<VaneBuilderConfigurator<T>> _vaneBuilderConfigurators;
        Func<VaneBuilder<T>> _builderFactory;

        protected VaneConfiguratorImpl()
            : this(DefaultSuccessVaneBuilder)
        {
        }

        protected VaneConfiguratorImpl(Func<VaneBuilder<T>> builderFactory)
        {
            _builderFactory = builderFactory;

            _vaneBuilderConfigurators = new List<VaneBuilderConfigurator<T>>();
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            if (_builderFactory == null)
                yield return this.Failure("BuilderFactory", "must not be null");

            foreach (ValidateResult result in _vaneBuilderConfigurators.SelectMany(x => x.Validate()))
                yield return result;
        }

        public void Add(VaneBuilderConfigurator<T> vaneBuilderConfigurator)
        {
            _vaneBuilderConfigurators.Add(vaneBuilderConfigurator);
        }

        static VaneBuilder<T> DefaultSuccessVaneBuilder()
        {
            return new SuccessBuilder<T>();
        }

        public Vane<T> CreateVane()
        {
            VaneBuilder<T> builder = _builderFactory();

            builder = _vaneBuilderConfigurators.Aggregate(builder, (x, configurator) => configurator.Configure(x));

            return builder.Build();
        }
    }
}