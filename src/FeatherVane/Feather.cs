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
    using Taskell;


    /// <summary>
    /// A FeatherVane is an autonomous vane that can be composed into a network of vanes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface Feather<T>
    {
        /// <summary>
        /// Builds a Task chain by enumerating the Vanes in the network
        /// </summary>
        /// <param name="composer">The Task builder</param>
        /// <param name="payload">The payload for this execution</param>
        /// <param name="next">The next Vane in the chain</param>
        void Compose(Composer composer, Payload<T> payload, Vane<T> next);
    }
}