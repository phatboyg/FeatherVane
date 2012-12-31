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
namespace FeatherVane
{
    using System;
    using FeatherVaneConfigurators;
    using SourceVaneConfigurators;


    public static class SpliceConfigurationExtensions
    {
        public static void Splice<T>(this VaneConfigurator<T> configurator,
            Action<SpliceSourceConfigurator<T>> configureCallback)
        {
            var spliceConfigurator = new SpliceSourceConfiguratorImpl<T>(configurator);

            configureCallback(spliceConfigurator);
        }

        public static void Source<T, TSource>(this SpliceSourceConfigurator<T> spliceSourceConfigurator,
            SourceVane<TSource> sourceVane, Action<SpliceConfigurator<T, TSource>> configureCallback)
        {
            var sourceVaneFactory = new ExistingSourceVaneFactory<TSource>(sourceVane);

            spliceSourceConfigurator.Source(sourceVaneFactory, configureCallback);
        }
    }
}