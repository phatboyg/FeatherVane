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
    using Payloads;

    public static class PayloadExtensions
    {
        /// <summary>
        /// Provide a default implementation of missing that returns null (perhaps an exception is a better
        /// approach since they should be using TryGet()
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TContext Get<TContext>(this Payload context)
            where TContext : class
        {
            return context.GetOrAdd(ContextNotFoundContextFactory<TContext>);
        }

        static TContext ContextNotFoundContextFactory<TContext>()
            where TContext : class
        {
            throw new ContextNotFoundException("No context factory provided.");
        }

        public static Payload<T> CreateProxy<T>(this Payload context, T body)
        {
            return new ProxyPayload<T>(context, body);
        }
    }
}