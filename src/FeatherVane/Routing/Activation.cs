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
namespace FeatherVane.Routing
{
    using System.Threading.Tasks;


    /// <summary>
    /// An activation is a identity node that can either be activated (RanToCompletion = true) or not (pending)
    /// </summary>
    public interface Activation
    {
        /// <summary>
        /// The identifier for the node that activated
        /// </summary>
        int Id { get; }

        /// <summary>
        /// The Task associated with the activation
        /// </summary>
        Task Task { get; }

        /// <summary>
        /// Complete the activation, if it has not already been completed
        /// </summary>
        void Complete();

        /// <summary>
        /// Cancel any pending activations that were not completed
        /// </summary>
        void Cancel();
    }
}