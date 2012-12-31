﻿// Copyright 2012-2012 Chris Patterson
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
namespace FeatherVane
{
    using System;
    using SourceVaneConfigurators;
    using Vanes;


    public static class FactorySourceConfigurationExtensions
    {
        public static void Factory<T, TSource>(this SpliceSourceConfigurator<T> configurator,
            Func<TSource> factoryMethod,
            Action<SourceVaneConfigurator<TSource>> configureCallback,
            Action<VaneConfigurator<Tuple<T, TSource>>> outputConfigureCallback)
        {
            if (configurator == null)
                throw new ArgumentNullException("configurator");
            if (configureCallback == null)
                throw new ArgumentNullException("configureCallback");

            var sourceConfigurator = new SourceVaneConfiguratorImpl<TSource>();

            sourceConfigurator.Factory(factoryMethod);

            configureCallback(sourceConfigurator);

            configurator.Source(sourceConfigurator, outputConfigureCallback);
        }

        /// <summary>
        /// Sets the source vane to a factory, using the factory method to create a new instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurator"></param>
        /// <param name="factoryMethod">The factory method for the source vane</param>
        public static void Factory<T>(this SourceVaneConfigurator<T> configurator, Func<T> factoryMethod)
        {
            if (configurator == null)
                throw new ArgumentNullException("configurator");

            configurator.UseSourceVaneFactory(() => new Factory<T>(factoryMethod));
        }
    }
}