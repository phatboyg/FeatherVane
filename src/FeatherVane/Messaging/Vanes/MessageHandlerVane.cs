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
namespace FeatherVane.Messaging.Vanes
{
    using System;


    /// <summary>
    /// Wraps a handler method into a FV so it can be executed
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    public class MessageHandlerVane<T> :
        FeatherVane<Message<T>>
        where T : class
    {
        readonly Action<Payload, Message<T>> _handlerMethod;

        public MessageHandlerVane(Action<Payload, Message<T>> handlerMethod)
        {
            _handlerMethod = handlerMethod;
        }

        void FeatherVane<Message<T>>.Compose(Composer composer, Payload<Message<T>> payload,
            Vane<Message<T>> next)
        {
            composer.Execute(() => _handlerMethod(payload, payload.Data));

            next.Compose(composer, payload);
        }
    }
}