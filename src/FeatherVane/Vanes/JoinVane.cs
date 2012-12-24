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


    public class JoinVane<TLeft, TRight> :
        FeatherVane<TLeft>
    {
        readonly SourceVane<TRight> _rightVane;
        readonly Vane<Tuple<TLeft, TRight>> _vane;

        public JoinVane(Vane<Tuple<TLeft, TRight>> vane, SourceVane<TRight> rightVane)
        {
            _vane = vane;
            _rightVane = rightVane;
        }

        void FeatherVane<TLeft>.Compose(Composer composer, Payload<TLeft> payload, Vane<TLeft> next)
        {
            composer.Execute(() =>
                {
                    var joiner = new Joiner(_vane, payload);
                    var taskComposer = new TaskComposer<TRight>(composer.CancellationToken);
                    _rightVane.Compose(taskComposer, payload, joiner);

                    return taskComposer.Complete();
                });

            next.Compose(composer, payload);
        }


        class Joiner :
            Vane<TRight>
        {
            readonly Payload<TLeft> _payload;
            readonly Vane<Tuple<TLeft, TRight>> _vane;

            public Joiner(Vane<Tuple<TLeft, TRight>> vane, Payload<TLeft> payload)
            {
                _vane = vane;
                _payload = payload;
            }

            public void Compose(Composer composer, Payload<TRight> payload)
            {
                composer.Execute(() =>
                    {
                        var joinPayload = new DelegatingPayload<Tuple<TLeft, TRight>>(_payload,
                            Tuple.Create(_payload.Data, payload.Data));

                        return TaskComposer.Compose(_vane, joinPayload, composer.CancellationToken);
                    });
            }
        }
    }
}