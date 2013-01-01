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
    using System.Collections.Generic;
    using Configurators;
    using FeatherVane.FeatherVaneBuilders;
    using FeatherVane.Vanes;
    using VaneBuilders;
    using Vanes;


    public class MessageConsumerBuilder<TMessage, TConsumer> :
        FeatherVaneBuilder<Message>,
        VaneBuilderConfigurator<Tuple<Message<TMessage>, TConsumer>>,
        FeatherVaneBuilder<Tuple<Message<TMessage>, TConsumer>>
        where TMessage : class
    {
        readonly Func<TConsumer, Action<Payload, Message<TMessage>>> _consumeMethod;
        readonly SourceVaneFactory<TConsumer> _sourceVaneFactory;
        readonly IList<VaneBuilderConfigurator<Tuple<Message<TMessage>, TConsumer>>> _vaneConfigurators;

        public MessageConsumerBuilder(SourceVaneFactory<TConsumer> sourceVaneFactory,
            IList<VaneBuilderConfigurator<Tuple<Message<TMessage>, TConsumer>>> vaneConfigurators,
            Func<TConsumer, Action<Payload, Message<TMessage>>> consumeMethod)
        {
            _sourceVaneFactory = sourceVaneFactory;
            _vaneConfigurators = vaneConfigurators;
            _consumeMethod = consumeMethod;
        }

        FeatherVane<Message> FeatherVaneBuilder<Message>.Build()
        {
            Vane<Tuple<Message<TMessage>, TConsumer>> messageVane =
                VaneFactory.New<Tuple<Message<TMessage>, TConsumer>>(x =>
                    {
                        foreach (var configurator in _vaneConfigurators)
                            x.Add(configurator);

                        x.Add(this);
                    });

            SourceVane<TConsumer> sourceVane = _sourceVaneFactory.Create();

            var spliceVane = new Splice<Message<TMessage>, TConsumer>(messageVane, sourceVane);
            Vane<Message<TMessage>> consumerVane = VaneFactory.Success(spliceVane);

            return new MessageType<TMessage>(consumerVane);
        }

        FeatherVane<Tuple<Message<TMessage>, TConsumer>> FeatherVaneBuilder<Tuple<Message<TMessage>, TConsumer>>.Build()
        {
            return new MessageConsumer<TMessage, TConsumer>(_consumeMethod);
        }

        void VaneBuilderConfigurator<Tuple<Message<TMessage>, TConsumer>>.Configure(
            VaneBuilder<Tuple<Message<TMessage>, TConsumer>> builder)
        {
            builder.Add(this);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            yield break;
        }
    }
}