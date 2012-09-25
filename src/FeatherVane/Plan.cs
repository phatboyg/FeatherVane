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
    public interface Plan
    {
        /// <summary>
        /// Execute the next step in the plan, if there are no more steps the plan executes the finishing move
        /// </summary>
        /// <returns></returns>
        bool Execute();

        /// <summary>
        /// Compensate the plan, executing the previous compensation step until the beginning is reached
        /// </summary>
        /// <returns></returns>
        bool Compensate();

        /// <summary>
        /// The number of steps in the plan
        /// </summary>
        int Length { get; }
    }

    public interface Plan<out T> :
        Plan
        where T : class
    {
        Payload<T> Payload { get; }
    }
}