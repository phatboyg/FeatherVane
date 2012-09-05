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
namespace FeatherVane
{
    using System;

    /// <summary>
    /// To avoid the bag of dicts approach, a VaneContext is used by vanes to
    /// provide access across layers (vanes) to common things. Think of it as 
    /// service location in the context of the handler.
    /// </summary>
    public interface Payload
    {
        /// <summary>
        /// The type of the generic context that is being passed by the vane
        /// </summary>
        Type BodyType { get; }

        /// <summary>
        /// The type of the Vane, include the Vane<typeparam name="T">T</typeparam> interface
        /// </summary>
        Type VaneType { get; }

        /// <summary>
        /// Checks if the context type is available
        /// </summary>
        /// <param name="contextType">The type of context requested</param>
        /// <returns>True if the context is available, otherwise false</returns>
        bool Has(Type contextType);

        /// <summary>
        /// Retrieve a context from a vane if it exists
        /// </summary>
        /// <typeparam name="TContext">The type of context to retrieve</typeparam>
        /// <param name="context">The resulting context if found</param>
        /// <returns>True if the context was found, otherwise false</returns>
        bool TryGet<TContext>(out TContext context)
            where TContext : class;

        /// <summary>
        /// Retrieve a context from a vane. If the context is not present, an argument
        /// exception is thrown unless a missing context provider is specified. Contexts
        /// can only be added using Get, think of it as a GetOrAdd operation. Once a context
        /// has been added using Get(), there is no way to replace that implementation.
        /// </summary>
        /// <typeparam name="TContext">The requested context</typeparam>
        /// <param name="contextFactory">The factory to create the context if it is not found</param>
        /// <returns>Either the existing or newly created context</returns>
        TContext GetOrAdd<TContext>(ContextFactory<TContext> contextFactory)
            where TContext : class;
    }

    /// <summary>
    /// The commonly used version of VaneContext, since the application will typically have
    /// a context of its own to promote through the chain as primary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface Payload<out T> :
        Payload
        where T : class
    {
        /// <summary>
        /// The body of the payload could a command, an event, the actual content of the request
        /// </summary>
        T Body { get; }
    }
}