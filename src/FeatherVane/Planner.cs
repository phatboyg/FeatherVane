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
    /// Planners are passed to vanes, which can choose to add steps to the plan for execution
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface Planner<T>
        where T : class
    {
        /// <summary>
        /// Add an item to the agenda
        /// </summary>
        /// <param name="agendaItem"></param>
        void Add(AgendaItem<T> agendaItem);

        /// <summary>
        /// Creates the agenda using the items added and the payload
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        Agenda<T> CreateAgenda(Payload<T> payload);
    }
}