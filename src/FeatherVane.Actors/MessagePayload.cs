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
namespace FeatherVane.Actors
{
    using System;
    using System.Reflection;
    using Execution;
    using FeatherVane.Payloads;

    /// <summary>
    /// Given a type T, provides a message payload for that type, allowing the message or any
    /// interface/type supported by the interface to be accessed as a context on the payload
    /// </summary>
    /// <typeparam name="TBody"></typeparam>
    public class MessagePayload<TBody> :
        Payload<Message>,
        Message<TBody>
        where TBody : class
    {
        readonly TBody _body;
        readonly Payload<Message> _payload;

        public MessagePayload(TBody body)
        {
            _payload = new PayloadImpl<Message>(this);
            _body = body;
        }

        MessagePayload(Payload<Message> payload, TBody body)
        {
            _payload = payload;
            _body = body;
        }

        bool Message.Has(Type messageType)
        {
#if !NETFX_CORE
            return messageType.IsInstanceOfType(_body);
#else
            return _body.GetType().GetTypeInfo().GetElementType() == messageType;
#endif
        }

        bool Message.TryGet<T>(out Message<T> context)
        {
            if (_body is T)
            {
                context = new MessagePayload<T>(_payload, _body as T);
                return true;
            }

            context = default(Message<T>);
            return false;
        }

        TBody Message<TBody>.Body
        {
            get { return _body; }
        }

        Type Payload.DataType
        {
            get { return _payload.DataType; }
        }

        Type Payload.VaneType
        {
            get { return _payload.VaneType; }
        }

        bool Payload.Has(Type contextType)
        {
            return _payload.Has(contextType);
        }

        bool Payload.TryGet<TContext>(out TContext context)
        {
            return _payload.TryGet(out context);
        }

        TContext Payload.GetOrAdd<TContext>(ContextFactory<TContext> contextFactory)
        {
            return _payload.GetOrAdd(contextFactory);
        }

        Message Payload<Message>.Data
        {
            get { return _payload.Data; }
        }
    }
}