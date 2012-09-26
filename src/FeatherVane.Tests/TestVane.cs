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
namespace FeatherVane.Tests
{
    using System.Diagnostics;

    [DebuggerNonUserCode]
    public class TestVane :
        FeatherVane<TestSubject>,
        AgendaItem<TestSubject>
    {
        public bool AssignCalled { get; set; }
        public bool ExecuteCalled { get; set; }
        public bool CompensateCalled { get; set; }

        public bool Execute(Agenda<TestSubject> agenda)
        {
            ExecuteCalled = true;

            return agenda.Execute();
        }

        public bool Compensate(Agenda<TestSubject> agenda)
        {
            CompensateCalled = true;

            return agenda.Compensate();
        }

        public Agenda<TestSubject> Plan(Planner<TestSubject> planner, Payload<TestSubject> payload,
            Vane<TestSubject> next)
        {
            AssignCalled = true;

            planner.Add(this);

            return next.Plan(planner, payload);
        }
    }
}