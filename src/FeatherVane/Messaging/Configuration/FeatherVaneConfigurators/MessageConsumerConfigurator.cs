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
    using FeatherVaneBuilders;
    using VaneBuilders;


    public class MessageConsumerConfigurator<TMessage, TConsumer> :
        VaneBuilderConfigurator<Tuple<Message, TConsumer>>
        where TMessage : class
    {
        readonly Func<TConsumer, Action<Payload, Message<TMessage>>> _consumeMethod;

        public MessageConsumerConfigurator(Func<TConsumer, Action<Payload, Message<TMessage>>> consumeMethod)
        {
            _consumeMethod = consumeMethod;
        }

        public void Configure(VaneBuilder<Tuple<Message, TConsumer>> builder)
        {
            builder.Add(new MessageConsumerBuilder<TMessage, TConsumer>(_consumeMethod));
        }

        public IEnumerable<ValidateResult> Validate()
        {
            if (_consumeMethod == null)
                yield return this.Failure("ConsumeMethod", "must not be null");
        }
    }
}