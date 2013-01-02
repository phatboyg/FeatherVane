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
namespace FeatherVane.NHibernateIntegration.SourceVaneConfigurators
{
    using System;
    using System.Collections.Generic;
    using Configurators;
    using FeatherVane.SourceVaneConfigurators;
    using FeatherVane.SourceVanes;
    using NHibernate;
    using SourceVanes;
    using VaneBuilders;
    using VaneConfigurators;
    using Vanes;


    public class LoadVaneConfiguratorImpl<T, TSource, TIdentity> :
        LoadVaneConfigurator<T, TSource>,
        LoadObjectVaneConfigurator<T, TSource, TIdentity>,
        VaneBuilderConfigurator<T>
    {
        readonly SourceVaneConfiguratorImpl<TIdentity> _identityConfigurator;
        readonly SourceVaneConfiguratorImpl<TSource> _missingConfigurator;
        readonly SourceVaneConfiguratorImpl<TSource> _sourceConfigurator;
        readonly VaneConfiguratorImpl<Tuple<T, TSource>> _vaneConfigurator;
        ISessionFactory _sessionFactory;

        public LoadVaneConfiguratorImpl()
        {
            _vaneConfigurator = new SuccessConfigurator<Tuple<T, TSource>>();
            _sourceConfigurator = new SourceVaneConfiguratorImpl<TSource>();
            _identityConfigurator = new SourceVaneConfiguratorImpl<TIdentity>();
            _missingConfigurator = new SourceVaneConfiguratorImpl<TSource>();

            _sourceConfigurator.UseSourceVane(CreateLoadVane);
            _missingConfigurator.UseSourceVane(() => new MissingSourceVane<TSource>());
        }

        void VaneConfigurator<TSource>.Add(VaneBuilderConfigurator<TSource> vaneBuilderConfigurator)
        {
            ((SourceVaneConfigurator<TSource>)_sourceConfigurator).Add(vaneBuilderConfigurator);
        }

        public void Id(Func<T, TIdentity> identitySelector)
        {
            _identityConfigurator.UseSourceVane(() => new IdentitySourceVane<T, TIdentity>(identitySelector));
        }

        public void Missing(Action<SourceVaneConfigurator<TSource>> configureMissing)
        {
            configureMissing(_missingConfigurator);
        }

        public void UseSessionFactory(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        void VaneConfigurator<Tuple<T, TSource>>.Add(VaneBuilderConfigurator<Tuple<T, TSource>> vaneBuilderConfigurator)
        {
            _vaneConfigurator.Add(vaneBuilderConfigurator);
        }

        void VaneBuilderConfigurator<T>.Configure(VaneBuilder<T> builder)
        {
            SourceVane<TSource> sourceVane = _sourceConfigurator.Create();
            Vane<Tuple<T, TSource>> outputVane = _vaneConfigurator.Create();

            var splice = new SpliceVane<T, TSource>(outputVane, sourceVane);

            builder.Add(splice);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            if (_sessionFactory == null)
                yield return this.Failure("SessionFactory", "must be specified");

            foreach (ValidateResult result in _vaneConfigurator.Validate())
                yield return result;
            foreach (ValidateResult result in _sourceConfigurator.Validate())
                yield return result;
        }

        SourceVane<TSource> CreateLoadVane()
        {
            SourceVane<TIdentity> identityVane = _identityConfigurator.Create();
            SourceVane<TSource> missingVane = _missingConfigurator.Create();
            return new LoadSourceVane<TSource, TIdentity>(_sessionFactory, identityVane, missingVane);
        }
    }
}