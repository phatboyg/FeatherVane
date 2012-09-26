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
    using System;

    public class ExecuteCompensateAction<T> :
        FeatherVane<T>,
        AgendaItem<T>
        where T : class
    {
        readonly Action<Agenda<T>> _compensateAction;
        readonly Action<Payload<T>> _executeAction;

        public ExecuteCompensateAction(Action<Payload<T>> executeAction, Action<Agenda<T>> compensateAction)
        {
            _executeAction = executeAction;
            _compensateAction = compensateAction;
        }

        public Agenda<T> AssignPlan(Planner<T> planner, Payload<T> payload, Vane<T> next)
        {
            planner.Add(this);

            return next.Plan(planner, payload);
        }

        public bool Execute(Agenda<T> agenda)
        {
            _executeAction(agenda.Payload);

            return agenda.Execute();
        }

        public bool Compensate(Agenda<T> agenda)
        {
            _compensateAction(agenda);

            return agenda.Compensate();
        }
    }
}