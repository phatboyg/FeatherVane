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
    using VaneBuilders;
    using Vanes;


    public class MessageHandlerConfigurator<T> :
        VaneBuilderConfigurator<Message>,
        VaneBuilderConfigurator<Message<T>>,
        VaneConfigurator<Message<T>>
        where T : class
    {
        readonly Action<Payload, Message<T>> _handlerMethod;
        readonly IList<VaneBuilderConfigurator<Message<T>>> _vaneConfigurators;

        public MessageHandlerConfigurator(Action<Payload, Message<T>> handlerMethod)
        {
            _handlerMethod = handlerMethod;
            _vaneConfigurators = new List<VaneBuilderConfigurator<Message<T>>>();
        }

        public void Configure(VaneBuilder<Message<T>> builder)
        {
            var handler = new MessageHandlerVane<T>(_handlerMethod);
            builder.Add(handler);
        }

        void VaneBuilderConfigurator<Message>.Configure(VaneBuilder<Message> builder)
        {
            Vane<Message<T>> handlerVane = ConfigureHandlerVane();

            var messageType = new MessageTypeVane<T>(handlerVane);
            builder.Add(messageType);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            return _vaneConfigurators.SelectMany(x => x.Validate());
        }

        void VaneConfigurator<Message<T>>.Add(VaneBuilderConfigurator<Message<T>> vaneBuilderConfigurator)
        {
            _vaneConfigurators.Add(vaneBuilderConfigurator);
        }

        Vane<Message<T>> ConfigureHandlerVane()
        {
            return VaneFactory.New<Message<T>>(x =>
                {
                    foreach (var configurator1 in _vaneConfigurators)
                        x.Add(configurator1);

                    x.Add(this);
                });
        }
    }
}