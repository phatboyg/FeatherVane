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


    public interface RoutingContext
    {
        /// <summary>
        /// Returns the segment at the specified position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        string Segment(int position);

        /// <summary>
        /// Activate a node, allowing other nodes to activate from it
        /// </summary>
        /// <param name="id">The identifier of the activated node</param>
        Task Activate(int id);

        /// <summary>
        /// Returns the specified activation. If the identifier was already activated, the previously
        /// activated Task is returned, otherwise, an incomplete activation is returned
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task GetActivation(int id);
    }
}