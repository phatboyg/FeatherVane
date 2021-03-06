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
namespace FeatherVane.Feathers
{
    using Taskell;


    /// <summary>
    /// If the next Vane faults, a Rescue reroutes the Build to an alternate Vane.
    /// </summary>
    /// <typeparam name="T">The Vane type</typeparam>
    public class RescueFeather<T> :
        Feather<T>,
        AcceptVaneVisitor
    {
        readonly Vane<T> _vane;

        /// <summary>
        /// Constructs a Rescue Vane
        /// </summary>
        /// <param name="vane">The rescue vane</param>
        public RescueFeather(Vane<T> vane)
        {
            _vane = vane;
        }

        bool AcceptVaneVisitor.Accept(VaneVisitor visitor)
        {
            return visitor.Visit(_vane);
        }

        void Feather<T>.Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            next.Compose(composer, payload);

            composer.Compensate(x => x.Task(CompositionExtensions.Compose(_vane, payload, composer.CancellationToken)));
        }
    }
}