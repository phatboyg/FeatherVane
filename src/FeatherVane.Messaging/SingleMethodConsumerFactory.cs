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
namespace FeatherVane.Messaging
{
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// A simple delegate-based consumer factory that has a single consumer method
    /// </summary>
    /// <typeparam name="TConsumer">The consumer type</typeparam>
    /// <typeparam name="TMessage">The message type for this consumer</typeparam>
    public class SingleMethodConsumerFactory<TConsumer, TMessage> :
        ConsumerFactory
        where TConsumer : class
        where TMessage : class
    {
        readonly Func<TConsumer> _factoryMethod;
        readonly Func<TConsumer, Payload<Message>, Message<TMessage>, Action> _methodSelector;

        public SingleMethodConsumerFactory(Func<TConsumer> factoryMethod,
            Func<TConsumer, Payload<Message>, Message<TMessage>, Action> methodSelector)
        {
            _factoryMethod = factoryMethod;
            _methodSelector = methodSelector;
        }

        public IEnumerable<Action> GetConsumers(Payload<Message> payload)
        {
            Message<TMessage> message;
            if (!payload.Data.TryGetAs(out message))
                yield break;

            yield return () =>
                {
                    TConsumer consumer = _factoryMethod();
                    if (consumer == null)
                    {
                        throw new ConsumerFactoryException(string.Format("Consumer factory returned null: {0}",
                            typeof(TConsumer).Name));
                    }

                    try
                    {
                        Action handler = _methodSelector(consumer, payload, message);
                        handler();
                    }
                    finally
                    {
                        var disposable = consumer as IDisposable;
                        if (disposable != null)
                            disposable.Dispose();
                    }
                };
        }
    }
}