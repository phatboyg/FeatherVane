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
namespace FeatherVane.Messaging.Vanes
{
    using System;


    /// <summary>
    /// Wraps a consumer factory and adds the consumer instance to the payload
    /// so that it can be invoked
    /// </summary>
    /// <typeparam name="TConsumer">The consumer type</typeparam>
    /// <typeparam name="T">The vane type</typeparam>
    public class MessageConsumerVane<TMessage, TConsumer> :
        FeatherVane<Tuple<Message<TMessage>, TConsumer>>
        where TMessage : class
    {
        readonly Func<TConsumer, Action<Payload, Message<TMessage>>> _selector;

        public MessageConsumerVane(Func<TConsumer, Action<Payload, Message<TMessage>>> selector)
        {
            _selector = selector;
        }

        public void Compose(Composer composer, Payload<Tuple<Message<TMessage>, TConsumer>> payload,
            Vane<Tuple<Message<TMessage>, TConsumer>> next)
        {
            composer.Execute(() =>
                {
                    Action<Payload, Message<TMessage>> handler = _selector(payload.Data.Item2);

                    handler(payload, payload.Data.Item1);
                });

            next.Compose(composer, payload);
        }
    }
}