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

    public static class VaneHandlerExtensions
    {
        /// <summary>
        /// Combines two VaneHandlers, calling them in order
        /// </summary>
        /// <param name="first"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static VaneHandler<T> CombineWith<T>(this VaneHandler<T> first, VaneHandler<T> next)
        {
            return new CombineVaneHandler<T>(first, next);
        }

        /// <summary>
        /// Intercept the handler with a delegate, which is responsible for calling the innerHandler
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="innerHandler"></param>
        /// <param name="interceptor"></param>
        /// <returns></returns>
        public static VaneHandler<T> InterceptWith<T>(this VaneHandler<T> innerHandler, Action<T, VaneHandler<T>> interceptor)
        {
            return new InterceptVaneHandler<T>(innerHandler, interceptor);
        }
    }
}