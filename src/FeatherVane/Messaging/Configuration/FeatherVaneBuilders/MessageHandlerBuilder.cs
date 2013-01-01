// Copyright 2012-2013 Chris Patterson
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
    using VaneBuilders;
    using Vanes;


    public class MessageHandlerBuilder<TMessage> :
        FeatherVaneBuilder<Message>,
        VaneBuilderConfigurator<Message<TMessage>>,
        FeatherVaneBuilder<Message<TMessage>>
        where TMessage : class
    {
        readonly Action<Payload, Message<TMessage>> _handlerMethod;
        readonly IList<VaneBuilderConfigurator<Message<TMessage>>> _vaneConfigurators;

        public MessageHandlerBuilder(IList<VaneBuilderConfigurator<Message<TMessage>>> vaneConfigurators,
            Action<Payload, Message<TMessage>> handlerMethod)
        {
            _vaneConfigurators = vaneConfigurators;
            _handlerMethod = handlerMethod;
        }

        FeatherVane<Message<TMessage>> FeatherVaneBuilder<Message<TMessage>>.Build()
        {
            return new MessageHandler<TMessage>(_handlerMethod);
        }

        FeatherVane<Message> FeatherVaneBuilder<Message>.Build()
        {
            Vane<Message<TMessage>> handlerVane =
                VaneFactory.New<Message<TMessage>>(x =>
                    {
                        foreach (var configurator in _vaneConfigurators)
                            x.Add(configurator);

                        x.Add(this);
                    });


            return new MessageType<TMessage>(handlerVane);
        }

        void VaneBuilderConfigurator<Message<TMessage>>.Configure(VaneBuilder<Message<TMessage>> builder)
        {
            builder.Add(this);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            yield break;
        }
    }
}