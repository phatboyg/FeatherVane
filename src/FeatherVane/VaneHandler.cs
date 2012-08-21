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
    using VaneHandlers;

    /// <summary>
    /// A Vane returns a VaneHandler when it is able to handle the inbound context. A
    /// VaneHandler can be anything from a simple delegate, to a more complex object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface VaneHandler<in T>
    {
        void Handle(T context);
    }

    /// <summary>
    /// Class used to create some of the more commonly used VaneHandler types.
    /// </summary>
    public static class VaneHandler
    {
        /// <summary>
        /// A simple no-operation, no-exception, do-nothing handler that merely
        /// indicates a successful operation.
        /// </summary>
        /// <typeparam name="T">The handler type</typeparam>
        /// <returns></returns>
        public static VaneHandler<T> Success<T>()
        {
            return new Success<T>();
        }

        public static VaneHandler<T> Unhandled<T>()
        {
            return new Unhandled<T>();
        }
    }
}