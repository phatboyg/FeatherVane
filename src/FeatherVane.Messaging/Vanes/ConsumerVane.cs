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
namespace FeatherVane.Messaging.Vanes
{
    using System;


    /// <summary>
    /// Creates a consumer for the message and adds it to the execution
    /// </summary>
    /// <typeparam name="TMessage">The message type for the consumer</typeparam>
    public class ConsumerMessageVane<TMessage> :
        FeatherVane<Message<TMessage>>
        where TMessage : class
    {
        readonly ConsumerFactory _consumerFactory;

        public ConsumerMessageVane(ConsumerFactory consumerFactory)
        {
            _consumerFactory = consumerFactory;
        }

        public void Compose(Composer composer, Payload<Message<TMessage>> payload, Vane<Message<TMessage>> next)
        {
            foreach (Action action in _consumerFactory.GetConsumers(payload))
            {
                Action consumerAction = action;
                composer.Execute(consumerAction);
            }

            next.Compose(composer, payload);
        }
    }
}