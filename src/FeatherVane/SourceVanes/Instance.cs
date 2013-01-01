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
namespace FeatherVane.SourceVanes
{
    using System;


    /// <summary>
    /// Wraps an instance into a SourceVane
    /// </summary>
    /// <typeparam name="T">The source type for the splice</typeparam>
    public class Instance<T> :
        SourceVane<T>
    {
        readonly T _instance;

        public Instance(T instance)
        {
            _instance = instance;
        }

        void SourceVane<T>.Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, T>> next)
        {
            composer.Execute(() =>
                {
                    Payload<Tuple<TPayload, T>> nextPayload = payload.CreateProxy(Tuple.Create(payload.Data, _instance));

                    return TaskComposer.Compose(next, nextPayload, composer.CancellationToken);
                });
        }
    }
}