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
namespace FeatherVane.Feathers
{
    using System;
    using Taskell;


    /// <summary>
    /// A Vane with an Execute and Compensate method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CompensateFeather<T> :
        Feather<T>
    {
        readonly Func<Payload<T>, bool> _compensate;

        /// <summary>
        /// Constructs a Execute and Compensate Vane
        /// </summary>
        /// <param name="compensate">A compensation, returns true if handled</param>
        public CompensateFeather(Func<Payload<T>, bool> compensate)
        {
            _compensate = compensate;
        }

        void Feather<T>.Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            next.Compose(composer, payload);

            composer.Compensate(compensation => _compensate(payload)
                                                    ? compensation.Handled()
                                                    : compensation.Throw());
        }
    }
}