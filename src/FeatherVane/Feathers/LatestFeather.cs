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


    /// <summary>
    /// Keeps track of the latest value received and uses it as the source for any source vane
    /// requests that arrive
    /// </summary>
    /// <typeparam name="T">The vane type</typeparam>
    public class LatestFeather<T> :
        Feather<T>,
        SourceVane<T>
        where T : class
    {
        T _latest;

        void Feather<T>.Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            _latest = payload.Data;

            next.Compose(composer, payload);
        }

        void SourceVane<T>.Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, T>> next)
        {
            Payload<Tuple<TPayload, T>> proxyPayload = payload.MergeRight(_latest);

            next.Compose(composer, proxyPayload);
        }
    }
}