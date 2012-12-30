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
    using VaneConfigurators;
    using Vanes;


    public static class VaneFactory
    {
        /// <summary>
        /// Configures a new vane using a fluent configuration syntax
        /// </summary>
        /// <typeparam name="T">The payload type for the vane</typeparam>
        /// <param name="configureCallback">The configuration callback</param>
        /// <returns>A newly constructed vane of the specified type</returns>
        public static Vane<T> New<T>(Action<VaneConfigurator<T>> configureCallback)
        {
            if (configureCallback == null)
                throw new ArgumentNullException("configureCallback");

            var configurator = new SuccessConfigurator<T>();

            configureCallback(configurator);

            configurator.ValidateConfigurator();

            return configurator.CreateVane();
        }


        /// <summary>
        /// Connects a sequence of FeatherVanes into a Vane that can be executed, with the default
        /// completion being success.
        /// </summary>
        /// <typeparam name="T">The Vane type</typeparam>
        /// <param name="vanes">The FeatherVanes to combine</param>
        /// <returns>An executable Vane</returns>
        public static Vane<T> Success<T>(params FeatherVane<T>[] vanes)
        {
            var success = new Success<T>();

            return Connect(success, vanes);
        }

        /// <summary>
        /// Connects a sequence of FeatherVanes into a Vane that can be executed, with the default
        /// completion being faulted.
        /// </summary>
        /// <typeparam name="T">The Vane type</typeparam>
        /// <param name="vanes">The FeatherVanes to combine</param>
        /// <returns>An executable Vane</returns>
        public static Vane<T> Unhandled<T>(params FeatherVane<T>[] vanes)
        {
            var unhandled = new Unhandled<T>();

            return Connect(unhandled, vanes);
        }

        public static SourceVane<T> Source<T>(SourceVane<T> source, params FeatherVane<T>[] vanes)
        {
            SourceVane<T> current = source;
            for (int i = 0; i < vanes.Length; i++)
                current = new NextSource<T>(current, vanes[i]);
            return current;
        }


        public static Vane<T> Connect<T>(Vane<T> last, params FeatherVane<T>[] vanes)
        {
            Vane<T> next = last;
            for (int i = vanes.Length - 1; i >= 0; i--)
                next = vanes[i].Append(next);

            return next;
        }

        public static Vane<T> Append<T>(this FeatherVane<T> head, Vane<T> next)
        {
            return new NextVane<T>(head, next);
        }

        public static Vane<T> Push<T>(this Vane<T> existing, FeatherVane<T> front)
        {
            return new NextVane<T>(front, existing);
        }
    }
}