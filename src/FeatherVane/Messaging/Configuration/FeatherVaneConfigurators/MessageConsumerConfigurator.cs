﻿// Copyright 2012-2013 Chris Patterson
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
namespace FeatherVane.Messaging.FeatherVaneConfigurators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configurators;
    using FeatherVane.FeatherVaneBuilders;
    using FeatherVaneBuilders;
    using VaneBuilders;


    public class MessageConsumerConfigurator<TMessage, TConsumer> :
        VaneBuilderConfigurator<Message>,
        VaneConfigurator<Tuple<Message<TMessage>, TConsumer>>
        where TMessage : class
    {
        readonly Func<TConsumer, Action<Payload, Message<TMessage>>> _consumeMethod;
        readonly SourceVaneFactory<TConsumer> _sourceVaneFactory;
        readonly IList<VaneBuilderConfigurator<Tuple<Message<TMessage>, TConsumer>>> _vaneConfigurators;

        public MessageConsumerConfigurator(SourceVaneFactory<TConsumer> sourceVaneFactory,
            Func<TConsumer, Action<Payload, Message<TMessage>>> consumeMethod)
        {
            _consumeMethod = consumeMethod;
            _sourceVaneFactory = sourceVaneFactory;
            _vaneConfigurators = new List<VaneBuilderConfigurator<Tuple<Message<TMessage>, TConsumer>>>();
        }

        public void Configure(VaneBuilder<Message> builder)
        {
            FeatherVaneBuilder<Message> messageConsumerBuilder =
                new MessageConsumerBuilder<TMessage, TConsumer>(_sourceVaneFactory,
                    _vaneConfigurators, _consumeMethod);

            builder.Add(messageConsumerBuilder);
        }

        public IEnumerable<ValidateResult> Validate()
        {
            return _vaneConfigurators.SelectMany(x => x.Validate());
        }

        public void Add(VaneBuilderConfigurator<Tuple<Message<TMessage>, TConsumer>> vaneBuilderConfigurator)
        {
            _vaneConfigurators.Add(vaneBuilderConfigurator);
        }
    }
}