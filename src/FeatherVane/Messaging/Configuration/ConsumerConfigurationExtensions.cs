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
namespace FeatherVane.Messaging
{
    using System;
    using FeatherVaneConfigurators;
    using SourceVaneConfigurators;


    public static class ConsumerConfigurationExtensions
    {
        public static void Consumer<TConsumer>(this VaneConfigurator<Message> configurator,
            Func<TConsumer> consumerFactory,
            Action<ConsumerConfigurator<TConsumer>> configureCallback)
        {
            var sourceVaneConfigurator = new SourceVaneConfiguratorImpl<TConsumer>();
            sourceVaneConfigurator.Factory(consumerFactory);

            var consumerConfigurator = new ConsumerConfiguratorImpl<TConsumer>(sourceVaneConfigurator);

            configureCallback(consumerConfigurator);

            configurator.Add(consumerConfigurator);
        }

        public static void Consume<T, TConsumer>(this VaneConfigurator<Tuple<Message<T>, TConsumer>> configurator,
            Func<TConsumer, Action<Payload, Message<T>>> consumeMethod)
            where T : class
        {
            var messageConfigurator = new ConsumeMessageConfigurator<T, TConsumer>(consumeMethod);

            configurator.Add(messageConfigurator);
        }
    }
}