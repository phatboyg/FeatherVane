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
namespace FeatherVane.Vanes
{
    using System;


    /// <summary>
    /// A Vane with an Execute and Compensate method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Compensate<T> :
        FeatherVane<T>
    {
        readonly Func<Payload<T>, bool> _compensate;

        /// <summary>
        /// Constructs a Execute and Compensate Vane
        /// </summary>
        /// <param name="compensate">A compensation, returns true if handled</param>
        public Compensate(Func<Payload<T>, bool> compensate)
        {
            _compensate = compensate;
        }

        void FeatherVane<T>.Build(Builder<T> builder, Payload<T> payload, Vane<T> next)
        {
            next.Build(builder, payload);

            builder.Compensate(compensation => _compensate(payload)
                                                   ? compensation.Handled()
                                                   : compensation.Throw());
        }
    }
}