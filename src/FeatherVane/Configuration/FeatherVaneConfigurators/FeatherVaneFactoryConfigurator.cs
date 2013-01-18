﻿// Copyright 2012-2013 Chris Patterson
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


    public class FeatherVaneFactoryConfigurator<T> :
        VaneBuilderConfigurator<T>
    {
        readonly Func<FeatherVane<T>> _factory;

        public FeatherVaneFactoryConfigurator(Func<FeatherVane<T>> factory)
        {
            _factory = factory;
        }

        public void Configure(VaneBuilder<T> builder)
        {
            FeatherVane<T> featherVane = _factory();

            builder.Add(featherVane);
        }

        public IEnumerable<ValidateResult> Validate()
        {
            if (_factory == null)
                yield return this.Failure("Factory", "must not be null");
        }
    }
}