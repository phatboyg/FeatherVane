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
    /// <summary>
    /// A route argument is captured by a parameter token in the route definition
    /// </summary>
    public interface RouteArgument
    {
        /// <summary>
        /// The name of the argument (captured from the parameter pattern typically
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The value of the argument capture by the parameter
        /// </summary>
        string Value { get; }
    }
}