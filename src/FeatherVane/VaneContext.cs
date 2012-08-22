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
    public interface VaneContext
    {
        /// <summary>
        /// The type of the generic context that is being passed by the vane
        /// </summary>
        Type ContextType { get; }

        /// <summary>
        /// Checks if the context type is available
        /// </summary>
        /// <param name="contextType">The type of context requested</param>
        /// <returns>True if the context is available, otherwise false</returns>
        bool HasContext(Type contextType);

        /// <summary>
        /// Retrieve a context from a vane if it exists
        /// </summary>
        /// <typeparam name="TContext">The type of context to retrieve</typeparam>
        /// <param name="context">The resulting context if found</param>
        /// <returns>True if the context was found, otherwise false</returns>
        bool TryGetContext<TContext>(out TContext context)
            where TContext : class;

        /// <summary>
        /// Retrieve a context from a vane. If the context is not present, an argument
        /// exception is thrown unless a missing context provider is specified.
        /// </summary>
        /// <typeparam name="TContext">The requested context</typeparam>
        /// <param name="missingContextProvider"></param>
        /// <returns></returns>
        TContext GetContext<TContext>(MissingContextProvider<TContext> missingContextProvider)
            where TContext : class;
    }

    /// <summary>
    /// The commonly used version of VaneContext, since the application will typically have
    /// a context of its own to promote through the chain as primary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface VaneContext<out T> :
        VaneContext
        where T : class
    {
        T Body { get; }
    }
}