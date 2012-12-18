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
    /// Executes a continuation as part of the Vane
    /// </summary>
    /// <typeparam name="T">The Vane type</typeparam>
    public class Execute<T> :
        FeatherVane<T>
    {
        readonly Action<Payload<T>> _continuation;

        public Execute(Action<Payload<T>> continuation)
        {
            _continuation = continuation;
        }

        public Execute(Action<T> continuation)
        {
            _continuation = payload => continuation(payload.Data);
        }

        void FeatherVane<T>.Build(Builder<T> builder, Payload<T> payload, Vane<T> next)
        {
            builder.Execute(() => _continuation(payload));

            next.Build(builder, payload);
        }
    }
}