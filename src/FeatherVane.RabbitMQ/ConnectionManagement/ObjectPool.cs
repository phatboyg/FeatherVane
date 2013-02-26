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
namespace FeatherVane.RabbitMQIntegration.ConnectionManagement
{
    using System;


    /// <summary>
    /// A generic object pool storing instances of an object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ObjectPool<T> :
        IDisposable
        where T : class
    {
        /// <summary>
        /// Returns an instance of an object from the pool
        /// </summary>
        /// <returns></returns>
        T Acquire();

        /// <summary>
        /// Releases an instance by reference
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="dispose">For the instance to be disposed rather than pooled</param>
        void Surrender(T instance, bool dispose = false);
    }
}