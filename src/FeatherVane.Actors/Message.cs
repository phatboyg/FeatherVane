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

    /// <summary>
    /// A message is passed through the system as a non-generic type, allowing
    /// specific types to be accessed through the message. Consider this a marker
    /// interface for a message
    /// </summary>
    public interface Message
    {
        /// <summary>
        /// Checks if the message can be cast to the specified type
        /// </summary>
        /// <param name="messageType">The message type</param>
        /// <returns>True if the message can be cast to the specified type, otherwise false</returns>
        bool Is(Type messageType);

        /// <summary>
        /// Retrieve the message as the specified type
        /// </summary>
        /// <typeparam name="TContext">The type of message to retrieve</typeparam>
        /// <param name="message">The resulting context if found</param>
        /// <returns>True if the context was found, otherwise false</returns>
        bool TryGetAs<T>(out Message<T> message)
            where T : class;
    }

    public interface Message<out T> :
        Message
        where T : class
    {
        /// <summary>
        /// The message body
        /// </summary>
        T Body { get; }
    }
}