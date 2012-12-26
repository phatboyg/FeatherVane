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
namespace FeatherVane.Tests.Messaging
{
    using System;
    using System.Threading;
    using FeatherVane.Messaging;
    using FeatherVane.Messaging.Payloads;
    using FeatherVane.Messaging.Vanes;
    using NUnit.Framework;
    using Vanes;
    using Visualization;


    [TestFixture]
    public class A_consumer_factory_vane
    {
        [Test]
        public void Should_support_delivery()
        {
            SourceVane<TestConsumer> factoryVane = SourceVaneFactory.New(() => new TestConsumer());

            Vane<Message<A>> messageAVane = factoryVane.New(x => x.Consumer<A>(v => v.Consume));
            var messageVane = new MessageVane<A>(messageAVane);

            Vane<Message<B>> messageBVane = factoryVane.New(x => x.Consumer<B>(v => v.Consume));
            var messageVaneB = new MessageVane<B>(messageBVane);

            var fanOutVane = new FanoutVane<Message>(new FeatherVane<Message>[] {messageVane, messageVaneB});
            Vane<Message> vane = VaneFactory.Success(fanOutVane);

            var a = new A {Value = "Hello"};
            Payload<Message> payload = new MessagePayload<A>(a);

            vane.Execute(payload, CancellationToken.None);

            var b = new B {Value = "World"};
            payload = new MessagePayload<B>(b);

            vane.Execute(payload, CancellationToken.None);

            var visitor = new StringVaneVisitor();
            visitor.Visit(vane);
            Console.WriteLine(visitor.ToString());
        }


        class TestConsumer
        {
            public void Consume(Payload payload, Message<A> message)
            {
                Console.WriteLine("A Value: {0}", message.Body.Value);
            }

            public void Consume(Payload payload, Message<B> message)
            {
                Console.WriteLine("B Value: {0}", message.Body.Value);
            }
        }


        class A
        {
            public string Value { get; set; }
        }


        class B
        {
            public string Value { get; set; }
        }
    }


    [TestFixture]
    public class A_failing_consumer
    {
        [Test]
        public void Should_be_disposed()
        {
            Vane<Message<A>> vane = SourceVaneFactory
                .New(() => new FailingConsumer())
                .New(x => x.Consumer<A>(v => v.Consume));


            var a = new A {Value = "Hello"};
            Payload<Message<A>> payload = new MessagePayload<A>(a);

            Assert.Throws<AggregateException>(() => vane.Execute(payload));
            Assert.IsTrue(FailingConsumer.Disposed, "Was not disposed");
        }


        class FailingConsumer :
            IDisposable
        {
            static bool _disposed;

            public FailingConsumer()
            {
                _disposed = false;
            }

            public static bool Disposed
            {
                get { return _disposed; }
            }

            public void Dispose()
            {
                _disposed = true;
            }

            public void Consume(Payload payload, Message<A> message)
            {
                throw new InvalidOperationException("This is an expected boom");
            }
        }


        class A
        {
            public string Value { get; set; }
        }
    }
}