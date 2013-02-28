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
namespace FeatherVane.Messaging.FeatherVaneConfigurators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configurators;
    using Factories;
    using FeatherVane.Feathers;
    using FeatherVane.Vanes;
    using Feathers;
    using VaneBuilders;


    public class MessageConsumerConfigurator<T, TConsumer> :
        VaneBuilderConfigurator<Message>,
        VaneBuilderConfigurator<Tuple<Message<T>, TConsumer>>,
        VaneConfigurator<Tuple<Message<T>, TConsumer>>
        where T : class
    {
        readonly Func<TConsumer, Action<Payload, Message<T>>> _consumeMethod;
        readonly SourceVaneFactory<TConsumer> _sourceVaneFactory;
        readonly IList<VaneBuilderConfigurator<Tuple<Message<T>, TConsumer>>> _vaneConfigurators;

        public MessageConsumerConfigurator(SourceVaneFactory<TConsumer> sourceVaneFactory,
            Func<TConsumer, Action<Payload, Message<T>>> consumeMethod)
        {
            _consumeMethod = consumeMethod;
            _sourceVaneFactory = sourceVaneFactory;
            _vaneConfigurators = new List<VaneBuilderConfigurator<Tuple<Message<T>, TConsumer>>>();
        }

        public void Configure(VaneBuilder<Message> builder)
        {
            Vane<Tuple<Message<T>, TConsumer>> messageVane = ConfigureMessageVane();

            SourceVane<TConsumer> sourceVane = _sourceVaneFactory.Create();

            var spliceVane = new SpliceFeather<Message<T>, TConsumer>(messageVane, sourceVane);
            Vane<Message<T>> consumerVane = VaneFactory.Success(spliceVane);

            var messageType = new MessageTypeFeather<T>(consumerVane);
            builder.Add(messageType);
        }

        public IEnumerable<ValidateResult> Validate()
        {
            return _vaneConfigurators.SelectMany(x => x.Validate());
        }

        public void Configure(VaneBuilder<Tuple<Message<T>, TConsumer>> builder)
        {
            var consumer = new MessageConsumerFeather<T, TConsumer>(_consumeMethod);
            builder.Add(consumer);
        }

        public void Add(VaneBuilderConfigurator<Tuple<Message<T>, TConsumer>> vaneBuilderConfigurator)
        {
            _vaneConfigurators.Add(vaneBuilderConfigurator);
        }

        Vane<Tuple<Message<T>, TConsumer>> ConfigureMessageVane()
        {
            return VaneFactory.New<Tuple<Message<T>, TConsumer>>(x =>
                {
                    foreach (var configurator1 in _vaneConfigurators)
                        x.Add(configurator1);

                    x.Add(this);
                });
        }
    }
}