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
namespace FeatherVane.Messaging.Feathers
{
    using System;


    /// <summary>
    /// Wraps a consumer factory and adds the consumer instance to the payload
    /// so that it can be invoked
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <typeparam name="TConsumer">The consumer type</typeparam>
    public class MessageConsumerFeather<T, TConsumer> :
        Feather<Tuple<Message<T>, TConsumer>>
        where T : class
    {
        readonly Func<TConsumer, Action<Payload, Message<T>>> _selector;

        public MessageConsumerFeather(Func<TConsumer, Action<Payload, Message<T>>> selector)
        {
            _selector = selector;
        }

        public void Compose(Composer composer, Payload<Tuple<Message<T>, TConsumer>> payload,
            Vane<Tuple<Message<T>, TConsumer>> next)
        {
            composer.Execute(() =>
                {
                    Action<Payload, Message<T>> handler = _selector(payload.Data.Item2);

                    handler(payload, payload.Data.Item1);
                });

            next.Compose(composer, payload);
        }
    }
}