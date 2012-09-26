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
namespace FeatherVane.Vanes
{
    public class Rescue<T> :
        FeatherVane<T>,
        AgendaItem<T>
    {
        readonly Vane<T> _rescueVane;

        public Rescue(Vane<T> rescueVane)
        {
            _rescueVane = rescueVane;
        }

        public Agenda<T> Plan(Planner<T> planner, Payload<T> payload, Vane<T> next)
        {
            planner.Add(this);

            return next.Plan(planner, payload);
        }

        public bool Execute(Agenda<T> agenda)
        {
            return agenda.Execute();
        }

        public bool Compensate(Agenda<T> agenda)
        {
            return _rescueVane.Execute(agenda.Payload);
        }
    }
}