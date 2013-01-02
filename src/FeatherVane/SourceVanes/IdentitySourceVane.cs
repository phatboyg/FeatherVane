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
    /// Selects an Id from an object
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class IdentitySourceVane<T, TId> :
        SourceVane<TId>
    {
        readonly Func<T, TId> _selector;

        public IdentitySourceVane(Func<T, TId> selector)
        {
            _selector = selector;
        }

        public void Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, TId>> next)
        {
            composer.Execute(() =>
                {
                    var objectPayload = payload as Payload<T>;
                    if (objectPayload == null)
                        throw new FeatherVaneException("Unable to map payload to " + typeof(T).Name);

                    TId id = _selector(objectPayload.Data);

                    Payload<Tuple<TPayload, TId>> nextPayload = payload.CreateProxy(Tuple.Create(payload.Data, id));

                    return TaskComposer.Compose(next, nextPayload, composer.CancellationToken);
                });
        }
    }
}