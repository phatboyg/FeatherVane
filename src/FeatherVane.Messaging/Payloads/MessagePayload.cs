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
namespace FeatherVane.Messaging.Payloads
{
    using FeatherVane.Payloads;


    /// <summary>
    /// Given a type T, provides a message payload for that type, allowing the message or any
    /// interface/type supported by the interface to be accessed as a context on the payload
    /// </summary>
    /// <typeparam name="TBody">The message body type</typeparam>
    public class MessagePayload<TBody> :
        ProxyPayload<Message<TBody>>
        where TBody : class
    {
        public MessagePayload(TBody body)
            : this(new ObjectMessage<TBody>(body))
        {
        }

        public MessagePayload(Message<TBody> body)
            : base(new PayloadImpl<Message<TBody>>(body), body)
        {
        }

        public MessagePayload(Payload originalPayload, Message<TBody> body)
            : base(originalPayload, body)
        {
        }
    }
}