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
        FeatherVaneBuilder<Message>
        where TMessage : class
    {
        readonly Func<TConsumer, Action<Payload, Message<TMessage>>> _consumeMethod;
        readonly SourceVaneFactory<TConsumer> _sourceVaneFactory;

        public MessageConsumerBuilder(SourceVaneFactory<TConsumer> sourceVaneFactory,
            Func<TConsumer, Action<Payload, Message<TMessage>>> consumeMethod)
        {
            _sourceVaneFactory = sourceVaneFactory;
            _consumeMethod = consumeMethod;
        }

        FeatherVane<Message> FeatherVaneBuilder<Message>.Build()
        {
            var messageConsumerVane = new MessageConsumerVane<TMessage, TConsumer>(_consumeMethod);
            Vane<Tuple<Message<TMessage>, TConsumer>> messageVane = VaneFactory.Success(messageConsumerVane);

            SourceVane<TConsumer> sourceVane = _sourceVaneFactory.Create();

            var spliceVane = new Splice<Message<TMessage>, TConsumer>(messageVane, sourceVane);
            Vane<Message<TMessage>> consumerVane = VaneFactory.Success(spliceVane);

            return new MessageVane<TMessage>(consumerVane);
        }
    }
}