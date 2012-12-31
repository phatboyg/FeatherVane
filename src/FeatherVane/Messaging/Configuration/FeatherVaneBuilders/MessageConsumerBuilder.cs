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
namespace FeatherVane.Messaging.FeatherVaneBuilders
{
    using System;
    using FeatherVane.FeatherVaneBuilders;
    using FeatherVane.Vanes;
    using Vanes;


    public class MessageConsumerBuilder<TMessage, TConsumer> :
        FeatherVaneBuilder<Tuple<Message, TConsumer>>
        where TMessage : class
    {
        readonly Func<TConsumer, Action<Payload, Message<TMessage>>> _consumeMethod;

        public MessageConsumerBuilder(Func<TConsumer, Action<Payload, Message<TMessage>>> consumeMethod)
        {
            _consumeMethod = consumeMethod;
        }

        public FeatherVane<Tuple<Message, TConsumer>> Build()
        {
            var messageConsumerVane = new MessageConsumerVane<TMessage, TConsumer>(_consumeMethod);
            var success = new Success<Tuple<Message<TMessage>, TConsumer>>();
            var nextVane = new NextVane<Tuple<Message<TMessage>, TConsumer>>(messageConsumerVane, success);

            var messageVane = new MessageVane<TMessage, TConsumer>(nextVane);
            return messageVane;
        }
    }
}