namespace FeatherVane.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class When_building_a_plan
    {
        [Test]
        public void Should_be_able_to_build()
        {
            FFVane<Address> fv = new Success();
            Payload<Address> payload = new PayloadImpl<Address>(new Address());

            var planner = new VanePlanner<Address>();

            var plan = fv.AssignPlan(planner, payload);

            Assert.IsNotNull(plan);

            Assert.IsTrue(plan.Execute());
        }

        [Test]
        public void Should_handle_false()
        {
            FFVane<Address> fv = new Fail();
            Payload<Address> payload = new PayloadImpl<Address>(new Address(){Street = "123"});

            var planner = new VanePlanner<Address>();

            var plan = fv.AssignPlan(planner, payload);

            Assert.IsNotNull(plan);

            Assert.IsFalse(plan.Execute());
        }

        [Test]
        public void Should_log_before_success()
        {
            FFVane<Address> success = new Success();
            FFFeatherVane<Address> log = new Logging();

            Payload<Address> payload = new PayloadImpl<Address>(new Address() { Street = "123" });

            var planner = new VanePlanner<Address>();

            var plan = log.AssignPlan(planner, payload, success);

            Assert.IsNotNull(plan);

            Assert.IsTrue(plan.Execute());
        }

        [Test]
        public void Should_log_and_compensate_for_fail()
        {
            FFVane<Address> fail = new Fail();
            FFFeatherVane<Address> log = new Logging();

            Payload<Address> payload = new PayloadImpl<Address>(new Address() { Street = "123" });

            var planner = new VanePlanner<Address>();

            var plan = log.AssignPlan(planner, payload, fail);

            Assert.IsNotNull(plan);

            Assert.IsFalse(plan.Execute());
        }


        class Logging :
            FFFeatherVane<Address>,
            Step<Address>
        {
            public Plan<Address> AssignPlan(Planner<Address> planner, Payload<Address> payload, FFVane<Address> next)
            {
                planner.Add(this);

                return next.AssignPlan(planner, payload);
            }

            public bool Execute(Plan<Address> plan)
            {
                Console.WriteLine("Logging: {0}", plan.Payload.Data.Street);

                return plan.Execute();
            }

            public bool Compensate(Plan<Address> plan)
            {
                Console.WriteLine("Compensating: {0}", plan.Payload.Data.Street);

                return plan.Compensate();
            }
        }


        class Fail :
            FFVane<Address>,
            Step<Address>
        {
            public Plan<Address> AssignPlan(Planner<Address> planner, Payload<Address> payload)
            {
                planner.Add(this);

                return planner.CreatePlan(payload);
            }

            public bool Execute(Plan<Address> plan)
            {
                return false;
            }

            public bool Compensate(Plan<Address> plan)
            {
                return false;
            }
        }

        class Success :
            FFVane<Address>
        {
            public Plan<Address> AssignPlan(Planner<Address> planner, Payload<Address> payload)
            {
                return planner.CreatePlan(payload);
            }
        }


        class Address
        {
            public string Street { get; set; }
        }
    }
}
