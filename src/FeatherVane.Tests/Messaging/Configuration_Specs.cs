namespace FeatherVane.Tests.Messaging
{
    using FeatherVane.Messaging;
    using NUnit.Framework;
    using Vanes;


    [TestFixture]
    public class Configuring_messaging_vanes
    {
        [Test]
        public void Should_support_a_consumer()
        {
            var vane = VaneFactory.New<Message>(x =>
                {
                    x.Consumer(() => new TestConsumer(), cx =>
                        {
                            cx.Consume<A>(c => c.Consume);
                            cx.Consume<B>(c => c.Consume);
                        });
                });

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<Splice<Message, TestConsumer>>(nextVane.FeatherVane);
            Assert.IsInstanceOf<Success<Message>>(nextVane.Next);
        }


        class TestConsumer
        {
            public void Consume(Payload payload, Message<A> message)
            {

            }

            public void Consume(Payload payload, Message<B> message)
            {

            }
        }


        class A
        {
        }


        class B

        {
        }
    }
}
