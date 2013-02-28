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
    using Configurators;
    using SourceVaneConfigurators;


    public static class SourceVaneFactory
    {
        /// <summary>
        /// Configure a SourceVane of the specified generic type
        /// </summary>
        /// <typeparam name="T">The SourceVane type</typeparam>
        /// <param name="configureCallback">The configuration callback</param>
        /// <returns>A ready to use SourceVane</returns>
        public static SourceVane<T> New<T>(Action<SourceVaneConfigurator<T>> configureCallback)
        {
            var configurator = new SourceVaneConfiguratorImpl<T>();

            configureCallback(configurator);

            return configurator.ValidateAndCreate();
        }
    }
}