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
    using System.Linq;
    using Configurators;
    using FeatherVaneBuilders;
    using VaneBuilders;


    public class ConsumerConfiguratorImpl<T> :
        ConsumerConfigurator<T>,
        VaneBuilderConfigurator<Message>
    {
        readonly IList<VaneBuilderConfigurator<Message>> _configurators;
        readonly SourceVaneFactory<T> _sourceVaneFactory;

        public ConsumerConfiguratorImpl(SourceVaneFactory<T> sourceVaneFactory)
        {
            _sourceVaneFactory = sourceVaneFactory;

            _configurators = new List<VaneBuilderConfigurator<Message>>();
        }

        void ConsumerConfigurator<T>.Consume<TMessage>(Func<T, Action<Payload, Message<TMessage>>> consumeMethod)
        {
            var messageConsumerConfigurator = new MessageConsumerConfigurator<TMessage, T>(_sourceVaneFactory,
                consumeMethod);

            _configurators.Add(messageConsumerConfigurator);
        }

        public void Consume<TMessage>(Func<T, Action<Payload, Message<TMessage>>> consumeMethod,
            Action<VaneConfigurator<Tuple<Message<TMessage>, T>>> configureCallback)
            where TMessage : class
        {
            var messageConsumerConfigurator = new MessageConsumerConfigurator<TMessage, T>(_sourceVaneFactory,
                consumeMethod);

            configureCallback(messageConsumerConfigurator);

            _configurators.Add(messageConsumerConfigurator);
        }

        void VaneBuilderConfigurator<Message>.Configure(VaneBuilder<Message> builder)
        {
            var consumerBuilder = new ConsumerBuilder<T>(_configurators);
            builder.Add(consumerBuilder);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            return _sourceVaneFactory.Validate().Concat(_configurators.SelectMany(x => x.Validate()));
        }
    }
}