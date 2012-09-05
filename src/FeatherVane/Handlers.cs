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
    using VaneHandlers;

    /// <summary>
    /// Class used to create some of the more commonly used VaneHandler types.
    /// </summary>
    public static class Handlers
    {
        /// <summary>
        /// A simple no-operation, no-exception, do-nothing handler that merely
        /// indicates a successful operation.
        /// </summary>
        /// <typeparam name="T">The handler type</typeparam>
        /// <returns></returns>
        public static Handler<T> Success<T>()
            where T : class
        {
            return new SuccessHandler<T>();
        }

        public static Handler<T> Unhandled<T>()
            where T : class
        {
            return new UnhandledHandler<T>();
        }

        public static Handler<T> Intercept<T>(Handler<T> handler, Action<Payload<T>, Handler<T>> interceptor)
            where T : class
        {
            return new InterceptHandler<T>(handler, interceptor);
        }
    }
}