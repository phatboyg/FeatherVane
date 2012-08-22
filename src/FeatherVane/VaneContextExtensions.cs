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

    public static class VaneContextExtensions
    {
        /// <summary>
        /// Provide a default implementation of missing that returns null (perhaps an exception is a better
        /// approach since they should be using TryGet()
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TContext GetContext<TContext>(this VaneContext context)
            where TContext : class
        {
            MissingContextProvider<TContext> missingContextProvider =
                () => { throw new ArgumentException("The specified context was not found.", "TContext"); };

            return context.GetContext(missingContextProvider);
        }

        public static VaneContext<T> CreateDelegatingVaneContext<T>(this VaneContext context, T body)
            where T : class
        {
            return new DelegatingVaneContext<T>(context, body);
        }
    }
}