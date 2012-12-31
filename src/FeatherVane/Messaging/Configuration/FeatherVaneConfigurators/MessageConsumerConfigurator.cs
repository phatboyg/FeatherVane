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
namespace FeatherVane.Messaging.FeatherVaneConfigurators
{
    using System;
    using System.Collections.Generic;
    using Configurators;
    using FeatherVane.FeatherVaneBuilders;
    using FeatherVaneBuilders;
    using VaneBuilders;


    public class MessageConsumerConfigurator<TMessage, TConsumer> :
        FeatherVaneBuilder<Message>,
        VaneBuilderConfigurator<Tuple<Message<TMessage>, TConsumer>>
        where TMessage : class
    {
        readonly Func<TConsumer, Action<Payload, Message<TMessage>>> _consumeMethod;
        readonly SourceVaneFactory<TConsumer> _sourceVaneFactory;

        public MessageConsumerConfigurator(SourceVaneFactory<TConsumer> sourceVaneFactory,
            Func<TConsumer, Action<Payload, Message<TMessage>>> consumeMethod)
        {
            _consumeMethod = consumeMethod;
            _sourceVaneFactory = sourceVaneFactory;
        }

        public FeatherVane<Message> Build()
        {
            FeatherVaneBuilder<Message> builder = new MessageConsumerBuilder<TMessage, TConsumer>(_sourceVaneFactory,
                _consumeMethod);

            return builder.Build();
        }


        public void Configure(VaneBuilder<Tuple<Message<TMessage>, TConsumer>> builder)
        {
        }

        public IEnumerable<ValidateResult> Validate()
        {
            if (_consumeMethod == null)
                yield return this.Failure("ConsumeMethod", "must not be null");
        }
    }
}