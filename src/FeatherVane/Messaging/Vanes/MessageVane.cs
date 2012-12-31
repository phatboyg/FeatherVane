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
    using Payloads;


    /// <summary>
    /// If a message can be converted to the message type of the vane, it will be
    /// executed by the fork in the vane before continuing
    /// </summary>
    /// <typeparam name="TMessage">The message type</typeparam>
    public class MessageVane<TMessage> :
        FeatherVane<Message>,
        AcceptVaneVisitor
        where TMessage : class
    {
        readonly Vane<Message<TMessage>> _vane;

        public MessageVane(Vane<Message<TMessage>> vane)
        {
            _vane = vane;
        }

        public bool Accept(VaneVisitor visitor)
        {
            return visitor.Visit(_vane);
        }

        void FeatherVane<Message>.Compose(Composer composer, Payload<Message> payload, Vane<Message> next)
        {
            composer.Execute(() =>
                {
                    Message<TMessage> message;
                    if (payload.Data.TryGetAs(out message))
                    {
                        var messagePayload = new MessagePayload<TMessage>(payload, message);

                        return TaskComposer.Compose(_vane, messagePayload, composer.CancellationToken);
                    }

                    return TaskComposer.Completed<Message>(composer.CancellationToken);
                });

            next.Compose(composer, payload);
        }
    }


    /// <summary>
    /// If a message can be converted to the message type of the vane, it will be
    /// executed by the fork in the vane before continuing
    /// </summary>
    /// <typeparam name="TMessage">The message type</typeparam>
    public class MessageVane<TMessage, T> :
        FeatherVane<Tuple<Message, T>>,
        AcceptVaneVisitor
        where TMessage : class
    {
        readonly Vane<Tuple<Message<TMessage>, T>> _vane;

        public MessageVane(Vane<Tuple<Message<TMessage>, T>> vane)
        {
            _vane = vane;
        }

        public bool Accept(VaneVisitor visitor)
        {
            return visitor.Visit(_vane);
        }

        void FeatherVane<Tuple<Message, T>>.Compose(Composer composer, Payload<Tuple<Message, T>> payload,
            Vane<Tuple<Message, T>> next)
        {
            composer.Execute(() =>
                {
                    Message<TMessage> message;
                    if (payload.Data.Item1.TryGetAs(out message))
                    {
                        Payload<Tuple<Message<TMessage>, T>> messagePayload =
                            payload.CreateProxy(Tuple.Create(message, payload.Data.Item2));

                        return TaskComposer.Compose(_vane, messagePayload, composer.CancellationToken);
                    }

                    return TaskComposer.Completed<Message>(composer.CancellationToken);
                });

            next.Compose(composer, payload);
        }
    }
}