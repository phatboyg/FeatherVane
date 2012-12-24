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
    using Payloads;

    /// <summary>
    /// Wraps a factory method into a SourceVane, allowing objects to be created within the
    /// scope of an execution with full lifecycle management.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FactoryVane<T> :
        SourceVane<T>
    {
        readonly Func<T> _factory;

        public FactoryVane(Func<T> factory)
        {
            _factory = factory;
        }

        void SourceVane<T>.Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<T> next)
        {
            T data = default(T);
            composer.Execute(() =>
                {
                    data = _factory();
                    var factoryPayload = new DelegatingPayload<T>(payload, data);

                    return TaskComposer.Compose(next, factoryPayload, composer.CancellationToken);
                });

            composer.Finally(() =>
                {
                    var disposable = data as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                });
        }
    }
}