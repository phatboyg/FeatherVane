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
namespace FeatherVane
{
    using System;
    using FeatherVaneConfigurators;


    public static class RescueConfigurationExtensions
    {
        public static void Rescue<T>(this VaneConfigurator<T> configurator, Action<VaneConfigurator<T>> configureRescue)
        {
            var rescueConfigurator = new RescueConfiguratorImpl<T>();

            configureRescue(rescueConfigurator);

            configurator.Add(rescueConfigurator);
        }

        public static void Rescue<T>(this VaneConfigurator<T> configurator, Func<Vane<T>> tailFactory,
            Action<VaneConfigurator<T>> configureRescue)
        {
            var rescueConfigurator = new RescueConfiguratorImpl<T>(tailFactory);

            configureRescue(rescueConfigurator);

            configurator.Add(rescueConfigurator);
        }
    }
}