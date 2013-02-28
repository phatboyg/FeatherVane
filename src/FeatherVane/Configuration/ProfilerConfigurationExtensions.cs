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


    public static class ProfilerConfigurationExtensions
    {
        /// <summary>
        /// Adds a Profiler vane
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurator"></param>
        /// <param name="configureCallback"></param>
        public static void Profiler<T>(this VaneConfigurator<T> configurator,
            Action<ProfilerConfigurator<T>> configureCallback)
        {
            var profilerConfigurator = new ProfilerConfiguratorImpl<T>();

            configureCallback(profilerConfigurator);

            configurator.Add(profilerConfigurator);
        }

        /// <summary>
        /// Configures the lower threshold for profile logging (only items that take more than timeout milliseconds are logged)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurator"></param>
        /// <param name="timeout">The timeout in milliseconds</param>
        /// <returns></returns>
        public static ProfilerConfigurator<T> Threshold<T>(this ProfilerConfigurator<T> configurator, int timeout)
        {
            return configurator.Threshold(TimeSpan.FromMilliseconds(timeout));
        }
    }
}