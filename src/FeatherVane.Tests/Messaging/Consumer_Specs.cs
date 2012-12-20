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
            ConsumerFactory factory = new SingleMethodConsumerFactory<TestConsumer, A>(() => new TestConsumer(),
                (c, p, m) => () => c.Consume(p, m));


            var consumerMessageVane = new ConsumerMessageVane<A>(factory);
            Vane<Message<A>> messageAVane = VaneFactory.Success(consumerMessageVane);

            var messageVane = new MessageVane<A>(messageAVane);

            var fanOutVane = new FanOutVane<Message>(new[] {messageVane});
            Vane<Message> vane = VaneFactory.Success(fanOutVane);

            var a = new A {Value = "Hello"};
            Payload<Message> payload = new MessagePayload<A>(a);

            vane.Execute(payload, CancellationToken.None);

            var visitor = new StringVaneVisitor();
            visitor.Visit(vane);
            Console.WriteLine(visitor.ToString());
        }


        class TestConsumer
        {
            public void Consume(Payload<Message> payload, Message<A> message)
            {
                Console.WriteLine("Value: {0}", message.Body.Value);

            }
        }


        class A
        {
            public string Value { get; set; }
        }
    }
}