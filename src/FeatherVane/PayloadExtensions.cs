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
namespace FeatherVane
{
    using System;
    using Internals.Extensions;
    using Payloads;


    public static class PayloadExtensions
    {
        /// <summary>
        /// Provide a default implementation of missing that returns null (perhaps an exception is a better
        /// approach since they should be using TryGet()
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static TContext Get<TContext>(this Payload payload)
            where TContext : class
        {
            if (payload == null)
                throw new ArgumentNullException("payload");

            return payload.GetOrAdd(ContextNotFoundContextFactory<TContext>);
        }

        static TContext ContextNotFoundContextFactory<TContext>()
            where TContext : class
        {
            throw new ContextNotFoundException("No context factory provided: " + typeof(TContext).GetTypeName());
        }

        /// <summary>
        /// Checks if the specified context type is available
        /// </summary>
        /// <typeparam name="T">The context type</typeparam>
        /// <param name="payload">The payload</param>
        /// <returns>True if the context is present, otherwise false</returns>
        public static bool Has<T>(this Payload payload)
        {
            if (payload == null)
                throw new ArgumentNullException("payload");

            return payload.Has(typeof(T));
        }

        public static Payload<T> CreateProxy<T>(this Payload context, T body)
        {
            return new ProxyPayload<T>(context, body);
        }

        /// <summary>
        /// Merge data into the left side of a payload, resulting in a Tuple payload
        /// </summary>
        /// <typeparam name="T">The payload type</typeparam>
        /// <typeparam name="TLeft">The new type to merge</typeparam>
        /// <param name="payload">The original payload</param>
        /// <param name="left">The new value to merge</param>
        /// <returns></returns>
        public static Payload<Tuple<TLeft, T>> MergeLeft<T, TLeft>(this Payload<T> payload, TLeft left)
        {
            return new ProxyPayload<Tuple<TLeft, T>>(payload, Tuple.Create(left, payload.Data));
        }

        public static Payload<Tuple<T, TRight>> MergeRight<T, TRight>(this Payload<T> payload, TRight right)
        {
            return new ProxyPayload<Tuple<T, TRight>>(payload, Tuple.Create(payload.Data, right));
        }

        public static Payload<TLeft> SplitLeft<TLeft, TRight>(this Payload<Tuple<TLeft, TRight>> payload)
        {
            return new ProxyPayload<TLeft>(payload, payload.Data.Item1);
        }

        public static Payload<TRight> SplitRight<TLeft, TRight>(this Payload<Tuple<TLeft, TRight>> payload)
        {
            return new ProxyPayload<TRight>(payload, payload.Data.Item2);
        }

        public static TRight Right<TLeft, TRight>(this Payload<Tuple<TLeft, TRight>> payload)
        {
            return payload.Data.Item2;
        }

        public static TLeft Left<TLeft, TRight>(this Payload<Tuple<TLeft, TRight>> payload)
        {
            return payload.Data.Item1;
        }
    }
}