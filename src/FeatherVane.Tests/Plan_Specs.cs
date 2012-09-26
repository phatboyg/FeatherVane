﻿// Copyright 2012-2012 Chris Patterson
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
    using NUnit.Framework;

    [TestFixture]
    public class When_building_a_plan
    {
        [Test]
        public void Should_create_the_plan_for_fail()
        {
            var fail = new Fail();
            Vane<Address> vane = Vane.Connect(fail);

            var exception = Assert.Throws<AgendaExecutionException>(() => vane.Execute(_address));

            Assert.AreEqual(0, exception.InnerExceptionCount);
        }

        [Test]
        public void Should_create_the_plan_for_success()
        {
            var success = new Success();

            Vane<Address> vane = Vane.Connect(success);
            vane.Execute(_address);

            Assert.IsTrue(success.AssignCalled);
        }

        [Test]
        public void Should_execute_and_compensate_on_fail()
        {
            var fail = new Fail();
            var log = new Logging();
            Vane<Address> vane = Vane.Connect(fail, log);

            var exception = Assert.Throws<AgendaExecutionException>(() => vane.Execute(_address));

            Assert.AreEqual(0, exception.InnerExceptionCount);

            Assert.IsTrue(fail.AssignCalled);
            Assert.IsTrue(fail.ExecuteCalled);
            Assert.IsFalse(fail.CompensateCalled);

            Assert.IsTrue(log.AssignCalled);
            Assert.IsTrue(log.ExecuteCalled);
            Assert.IsTrue(log.CompensateCalled);
        }

        [Test]
        public void Should_execute_two_and_compensate_on_fail()
        {
            var fail = new Fail();
            var log = new Logging();
            var log2 = new Logging();
            Vane<Address> vane = Vane.Connect(fail, log, log2);

            var exception = Assert.Throws<AgendaExecutionException>(() => vane.Execute(_address));

            Assert.AreEqual(0, exception.InnerExceptionCount);

            Assert.IsTrue(fail.AssignCalled);
            Assert.IsTrue(fail.ExecuteCalled);
            Assert.IsFalse(fail.CompensateCalled);

            Assert.IsTrue(log.AssignCalled);
            Assert.IsTrue(log.ExecuteCalled);
            Assert.IsTrue(log.CompensateCalled);

            Assert.IsTrue(log2.AssignCalled);
            Assert.IsTrue(log2.ExecuteCalled);
            Assert.IsTrue(log2.CompensateCalled);
        }

        [Test]
        public void Should_execute_both_vanes()
        {
            var success = new Success();
            var log = new Logging();
            Vane<Address> vane = Vane.Connect(success, log);

            vane.Execute(_address);

            Assert.IsTrue(success.AssignCalled);

            Assert.IsTrue(log.AssignCalled);
            Assert.IsTrue(log.ExecuteCalled);
            Assert.IsFalse(log.CompensateCalled);
        }

        [Test]
        public void Should_execute_three_vanes()
        {
            var success = new Success();
            var log = new Logging();
            var log2 = new Logging();
            Vane<Address> vane = Vane.Connect(success, log, log2);

            vane.Execute(_address);

            Assert.IsTrue(success.AssignCalled);

            Assert.IsTrue(log.AssignCalled);
            Assert.IsTrue(log.ExecuteCalled);
            Assert.IsFalse(log.CompensateCalled);

            Assert.IsTrue(log2.AssignCalled);
            Assert.IsTrue(log2.ExecuteCalled);
            Assert.IsFalse(log2.CompensateCalled);
        }

        Address _address = new Address {Street = "123"};


        class Logging :
            FeatherVane<Address>,
            AgendaItem<Address>
        {
            public bool AssignCalled { get; set; }
            public bool ExecuteCalled { get; set; }
            public bool CompensateCalled { get; set; }

            public bool Execute(Agenda<Address> agenda)
            {
                ExecuteCalled = true;

                return agenda.Execute();
            }

            public bool Compensate(Agenda<Address> agenda)
            {
                CompensateCalled = true;

                return agenda.Compensate();
            }

            public Agenda<Address> AssignPlan(Planner<Address> planner, Payload<Address> payload, Vane<Address> next)
            {
                AssignCalled = true;

                planner.Add(this);

                return next.Plan(planner, payload);
            }
        }


        class Fail :
            Vane<Address>,
            AgendaItem<Address>
        {
            public bool AssignCalled { get; set; }
            public bool ExecuteCalled { get; set; }
            public bool CompensateCalled { get; set; }

            public bool Execute(Agenda<Address> agenda)
            {
                ExecuteCalled = true;
                return false;
            }

            public bool Compensate(Agenda<Address> agenda)
            {
                CompensateCalled = true;

                return agenda.Compensate();
            }

            public Agenda<Address> Plan(Planner<Address> planner, Payload<Address> payload)
            {
                planner.Add(this);

                AssignCalled = true;

                return planner.CreatePlan(payload);
            }
        }

        class Success :
            Vane<Address>
        {
            public bool AssignCalled { get; set; }

            public Agenda<Address> Plan(Planner<Address> planner, Payload<Address> payload)
            {
                AssignCalled = true;

                return planner.CreatePlan(payload);
            }
        }


        class Address
        {
            public string Street { get; set; }
        }
    }
}