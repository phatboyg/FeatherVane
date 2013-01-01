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
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TObject"></typeparam>
    public class Id<TObject, T> :
        SourceVane<T>
    {
        readonly Func<TObject, T> _provider;

        public Id(Func<TObject, T> provider)
        {
            _provider = provider;
        }

        public void Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, T>> next)
        {
            composer.Execute(() =>
                {
                    var objectPayload = payload as Payload<TObject>;
                    if (objectPayload == null)
                        throw new FeatherVaneException("Unable to map payload to " + typeof(TObject).Name);

                    T data = _provider(objectPayload.Data);

                    Payload<Tuple<TPayload, T>> nextPayload = payload.CreateProxy(Tuple.Create(payload.Data, data));

                    return TaskComposer.Compose(next, nextPayload, composer.CancellationToken);
                });
        }
    }
}