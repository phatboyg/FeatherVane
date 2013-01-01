﻿namespace FeatherVane.Tests.Messaging
{
    using System;
    using System.IO;
    using FeatherVane.Messaging;
    using NUnit.Framework;
    using Vanes;
    using Visualizer;


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
                            cx.Consume<B>(c => c.Consume, mx =>
                                {
                                    mx.ConsoleLogger(v => string.Format("Logging: {0}", v.Data.Item2.Id));
                                });
                        });
                });

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<Fanout<Message>>(nextVane.FeatherVane);
            Assert.IsInstanceOf<Success<Message>>(nextVane.Next);

            var fanoutVane = nextVane.FeatherVane as Fanout<Message>;
            Assert.IsNotNull(fanoutVane);

            Assert.AreEqual(2, fanoutVane.Count);

            vane.RenderGraphToFile(new FileInfo("messageVaneGraph.png"));
        }

        [Test]
        public void Should_support_an_instance()
        {
            var vane = VaneFactory.New<Message>(x =>
                {
                    x.Instance(new TestConsumer(), cx =>
                        {
                            cx.Consume<A>(c => c.Consume);
                            cx.Consume<B>(c => c.Consume);
                        });
                });

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<Fanout<Message>>(nextVane.FeatherVane);
            Assert.IsInstanceOf<Success<Message>>(nextVane.Next);

            var fanoutVane = nextVane.FeatherVane as Fanout<Message>;
            Assert.IsNotNull(fanoutVane);

            Assert.AreEqual(2, fanoutVane.Count);

            vane.RenderGraphToFile(new FileInfo("messageVaneGraph.png"));
        }

        [Test]
        public void Should_support_a_handler()
        {
            var vane = VaneFactory.New<Message>(x =>
                {
                    x.Fanout(fx =>
                        {
                            fx.Handler<A>((payload, message) => { });
                            fx.Handler<B>((payload, message) => { }, hx => hx.ConsoleLogger(v => ""));
                        });
                });

            vane.RenderGraphToFile(new FileInfo("messageVaneGraph.png"));
            
            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<Fanout<Message>>(nextVane.FeatherVane);
            Assert.IsInstanceOf<Success<Message>>(nextVane.Next);

            var fanoutVane = nextVane.FeatherVane as Fanout<Message>;
            Assert.IsNotNull(fanoutVane);

            Assert.AreEqual(2, fanoutVane.Count);

        }


        class TestConsumer
        {
            public Guid Id = Guid.NewGuid();

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
