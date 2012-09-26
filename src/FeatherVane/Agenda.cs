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
    /// <summary>
    /// An Agenda contains a list of items to be executed. The items are executed sequentially,
    /// and completes once the final agenda item is executed. If an exception occurs within the 
    /// agenda, previously executed agenda items are given the opportunity to compensate their
    /// actions.
    /// </summary>
    public interface Agenda
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
    }

    public interface Agenda<out T> :
        Agenda
        where T : class
    {
        /// <summary>
        /// The payload contained in this agenda instance
        /// </summary>
        Payload<T> Payload { get; }
    }
}