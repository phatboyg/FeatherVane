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
    using System;

    /// <summary>
    /// An object-based message, where the message is an instance of an object and
    /// any related message types are handled by reflection.
    /// </summary>
    /// <typeparam name="TBody">The type of the message</typeparam>
    public class ObjectMessage<TBody> :
        Message<TBody>
        where TBody : class
    {
        readonly TBody _body;

        public ObjectMessage(TBody body)
        {
            _body = body;
        }

        bool Message.Is(Type messageType)
        {
            return messageType.IsInstanceOfType(_body);
        }

        bool Message.TryGetAs<T>(out Message<T> message)
        {
            var body = _body as T;
            if (body != null)
            {
                message = new ObjectMessage<T>(body);
                return true;
            }

            message = default(Message<T>);
            return false;
        }

        TBody Message<TBody>.Body
        {
            get { return _body; }
        }
    }
}