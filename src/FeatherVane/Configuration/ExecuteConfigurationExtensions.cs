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
    using System.Threading.Tasks;
    using FeatherVaneConfigurators;
    using VaneConfigurators;


    public static class ExecuteConfigurationExtensions
    {
        /// <summary>
        /// Execute a continuation (synchronously if possible)
        /// </summary>
        /// <typeparam name="T">The vane type</typeparam>
        /// <param name="configurator">The vane configurator</param>
        /// <param name="continuation">The continuation to execute</param>
        public static void Execute<T>(this VaneConfigurator<T> configurator, Action<Payload<T>> continuation)
        {
            if (configurator == null)
                throw new ArgumentNullException("configurator");

            var vaneConfigurator = new ExecuteConfigurator<T>(continuation);

            configurator.Add(vaneConfigurator);
        }

        /// <summary>
        /// Execute a Task (returned by the specified continuation)
        /// </summary>
        /// <typeparam name="T">The vane type</typeparam>
        /// <param name="configurator">The vane configurator</param>
        /// <param name="continuationTask">The continuation to create the Task</param>
        public static void Execute<T>(this VaneConfigurator<T> configurator, Func<Payload<T>, Task> continuationTask)
        {
            if (configurator == null)
                throw new ArgumentNullException("configurator");

            var vaneConfigurator = new ExecuteTaskConfigurator<T>(continuationTask);

            configurator.Add(vaneConfigurator);
        }
    }
}