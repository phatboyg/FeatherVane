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
    /// Splices a vane with a source vane
    /// </summary>
    /// <typeparam name="T">The primary vane type</typeparam>
    /// <typeparam name="TSource">The source vane type</typeparam>
    public class SpliceVane<T, TSource> :
        FeatherVane<T>
    {
        readonly SourceVane<TSource> _sourceVane;
        readonly Vane<Tuple<T, TSource>> _vane;

        public SpliceVane(Vane<Tuple<T, TSource>> vane, SourceVane<TSource> sourceVane)
        {
            _vane = vane;
            _sourceVane = sourceVane;
        }

        void FeatherVane<T>.Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            composer.Execute(() =>
                {
                    var splice = new Splice(_vane, payload);

                    return TaskComposer.Compose(_sourceVane, payload, splice, composer.CancellationToken);
                });

            next.Compose(composer, payload);
        }


        class Splice :
            Vane<TSource>
        {
            readonly Payload<T> _payload;
            readonly Vane<Tuple<T, TSource>> _vane;

            public Splice(Vane<Tuple<T, TSource>> vane, Payload<T> payload)
            {
                _vane = vane;
                _payload = payload;
            }

            public void Compose(Composer composer, Payload<TSource> payload)
            {
                composer.Execute(() =>
                    {
                        Tuple<T, TSource> body = Tuple.Create(_payload.Data, payload.Data);

                        Payload<Tuple<T, TSource>> vanePayload = payload.CreateProxy(body);

                        return TaskComposer.Compose(_vane, vanePayload, composer.CancellationToken);
                    });
            }
        }
    }
}