namespace FeatherVane.Tests.Messaging
{
    using System;
    using FeatherVane.Messaging;
    using FeatherVane.Messaging.Payloads;
    using FeatherVane.Messaging.Vanes;
    using NUnit.Framework;
    using Vanes;


    [TestFixture]
    public class Using_a_source_vane_through_a_join
    {
        [Test]
        public void Should_properly_generate_the_output_types()
        {
            var messageConsumerVane = new MessageConsumerVane<A, WorkingConsumer>(x => x.Consume);
            Vane<Tuple<Message<A>, WorkingConsumer>> consumerVane = VaneFactory.Success(messageConsumerVane);
           
            var factoryVane = new FactoryVane<WorkingConsumer>(() => new WorkingConsumer());
            var joinVane = new JoinVane<Message<A>, WorkingConsumer>(consumerVane, factoryVane);


            var vane = VaneFactory.Success(joinVane);

            var a = new A { Value = "Hello" };
            Payload<Message<A>> payload = new MessagePayload<A>(a);

            vane.Execute(payload);

            Assert.IsTrue(WorkingConsumer.Called, "Was not called");
        }

        class WorkingConsumer 
        {
            static bool _called;

            public static bool Called
            {
                get { return _called; }
            }

            public WorkingConsumer()
            {
                _called = false;
            }

            public void Consume(Payload payload, Message<A> message)
            {
                _called = true;
            }
        }

        class A
        {
            public string Value { get; set; }
        }
    }
}
