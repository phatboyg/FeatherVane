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
    using FeatherVane.Messaging;
    using FeatherVane.Messaging.Payloads;
    using FeatherVane.Messaging.Vanes;
    using Vanes;


    public class ConsumerThroughput :
        Throughput
    {
        readonly Vane<Message> _vane;

        public ConsumerThroughput()
        {
            ConsumerFactory factory = new SingleMethodConsumerFactory<TestConsumer, Subject>(() => new TestConsumer(),
                (c, p, m) => () => c.Consume(p, m));


            var consumerMessageVane = new ConsumerMessageVane<Subject>(factory);
            Vane<Message<Subject>> messageAVane = VaneFactory.Success(consumerMessageVane);

            var messageVane = new MessageVane<Subject>(messageAVane);

            var fanOutVane = new FanOutVane<Message>(new[] {messageVane});


            _vane = VaneFactory.Success(fanOutVane);
        }

        public void Execute(Subject subject)
        {
            var messagePayload = new MessagePayload<Subject>(subject);

            _vane.Execute(messagePayload);
        }


        class TestConsumer
        {
            public void Consume(Payload<Message> payload, Message<Subject> message)
            {
            }
        }
    }
}