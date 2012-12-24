﻿// Copyright 2012-2012 Chris Patterson
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


    /// <summary>
    /// A simple delegate-based consumer factory that has a single consumer method
    /// </summary>
    /// <typeparam name="TConsumer">The consumer type</typeparam>
    public class DelegateConsumerFactory<TConsumer> :
        ConsumerFactory<TConsumer>
    {
        readonly Func<TConsumer> _factoryMethod;

        public DelegateConsumerFactory(Func<TConsumer> factoryMethod)
        {
            _factoryMethod = factoryMethod;
        }

        public Consumer<TConsumer> GetConsumer<T>(Payload<T> payload)
        {
            TConsumer consumer = _factoryMethod();

            var consumerImpl = new ConsumerImpl<TConsumer>(consumer);

            return consumerImpl;
        }
    }
}