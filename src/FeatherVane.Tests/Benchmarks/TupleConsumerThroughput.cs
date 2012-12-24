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
namespace FeatherVane.Tests.Benchmarks
{
    using System;
    using FeatherVane.Messaging;
    using FeatherVane.Messaging.Payloads;
    using FeatherVane.Messaging.Vanes;
    using Vanes;


    public class TupleConsumerThroughput :
        Throughput
    {
        readonly Vane<Message> _vane;

        public TupleConsumerThroughput()
        {
            var messageConsumerVane = new MessageConsumerVane<Subject, TestConsumer>(x => x.Consume);
            Vane<Tuple<Message<Subject>, TestConsumer>> consumerVane = VaneFactory.Success(messageConsumerVane);

            var factoryVane = new FactoryVane<TestConsumer>(() => new TestConsumer());
            var joinVane = new JoinVane<Message<Subject>, TestConsumer>(consumerVane, factoryVane);

            Vane<Message<Subject>> vane = VaneFactory.Success(joinVane);


            var messageVane = new MessageVane<Subject>(vane);

            var fanOutVane = new FanoutVane<Message>(new[] {messageVane});

            _vane = VaneFactory.Success(fanOutVane);
        }

        public void Execute(Subject subject)
        {
            var messagePayload = new MessagePayload<Subject>(subject);

            _vane.Execute(messagePayload);
        }


        class TestConsumer
        {
            public void Consume(Payload payload, Message<Subject> message)
            {
            }
        }
    }
}