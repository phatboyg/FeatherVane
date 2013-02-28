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
namespace FeatherVane.Messaging.Feathers
{
    using FeatherVane.Messaging.Payloads;


    /// <summary>
    /// If a message can be converted to the message type of the vane, it will be
    /// executed by the fork in the vane before continuing
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    public class MessageTypeFeather<T> :
        Feather<Message>,
        AcceptVaneVisitor
        where T : class
    {
        readonly Vane<Message<T>> _vane;

        public MessageTypeFeather(Vane<Message<T>> vane)
        {
            _vane = vane;
        }

        public bool Accept(VaneVisitor visitor)
        {
            return visitor.Visit(_vane);
        }

        void Feather<Message>.Compose(Composer composer, Payload<Message> payload, Vane<Message> next)
        {
            composer.Execute(() =>
                {
                    Message<T> message;
                    if (payload.Data.TryGetAs(out message))
                    {
                        var messagePayload = new MessagePayload<T>(payload, message);

                        return TaskComposer.Compose(_vane, messagePayload, composer.CancellationToken);
                    }

                    return TaskComposer.Completed<Message>(composer.CancellationToken);
                });

            next.Compose(composer, payload);
        }
    }
}