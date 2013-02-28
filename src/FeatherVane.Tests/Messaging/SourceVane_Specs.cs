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
    using System.IO;
    using FeatherVane.Messaging;
    using FeatherVane.Messaging.Feathers;
    using FeatherVane.Messaging.Payloads;
    using Feathers;
    using NUnit.Framework;
    using SourceVanes;
    using Vanes;
    using Visualization;
    using Visualizer;


    [TestFixture]
    public class Using_a_source_vane_through_a_join
    {
        [Test]
        public void Should_properly_generate_the_output_types()
        {
            var a = new A {Value = "Hello"};
            Payload<Message<A>> payload = new MessagePayload<A>(a);

            _vane.Execute(payload);

            Assert.IsTrue(WorkingConsumer.Called, "Was not called");
        }


        [Test, Explicit]
        public void Should_render_graph_nicely()
        {
            _vane.RenderGraphToFile(new FileInfo("sourceVaneGraph.png"));
        }

        [Test, Explicit]
        public void Should_show()
        {
            var graphVisitor = new GraphVaneVisitor();
            graphVisitor.Visit(_vane);

            FeatherVaneGraph graph = graphVisitor.GetGraphData();
            VaneDebugVisualizer.TestShowVisualizer(graph);
        }

        Vane<Message> _vane;

        [TestFixtureSetUp]
        public void Setup()
        {
            var messageConsumerVane = new MessageConsumerFeather<A, WorkingConsumer>(x => x.Consume);
            Vane<Tuple<Message<A>, WorkingConsumer>> consumerVane = VaneFactory.Success(messageConsumerVane);

            var messageConsumerVaneB = new MessageConsumerFeather<B, WorkingConsumer>(x => x.Consume);
            Vane<Tuple<Message<B>, WorkingConsumer>> consumerVaneB = VaneFactory.Success(messageConsumerVaneB);

            var factoryVane = new FactorySourceVane<WorkingConsumer>(() => new WorkingConsumer());
            var loggerVane = new LogFeather<WorkingConsumer>(Console.Out, x => "Logging");
            var profilerVane = new ProfilerFeather<WorkingConsumer>(Console.Out, TimeSpan.FromMilliseconds(1));

            var sourceVane = VaneFactory.Source(factoryVane, loggerVane, profilerVane);
            var spliceVane = new SpliceFeather<Message<A>, WorkingConsumer>(consumerVane, sourceVane);

            var messageVane = new MessageTypeFeather<A>(VaneFactory.Success(spliceVane));

            var spliceVaneB = new SpliceFeather<Message<B>, WorkingConsumer>(consumerVaneB, sourceVane);
            var messageVaneB = new MessageTypeFeather<B>(VaneFactory.Success(spliceVaneB));

            var fanOutVane = new FanoutFeather<Message>(new Feather<Message>[] { messageVane, messageVaneB });
            
            _vane = VaneFactory.Success(fanOutVane);
        }


        class WorkingConsumer
        {
            static bool _called;

            public WorkingConsumer()
            {
                _called = false;
            }

            public static bool Called
            {
                get { return _called; }
            }

            public void Consume(Payload payload, Message<A> message)
            {
                Console.WriteLine(message.Body.Value);

                _called = true;
            }

            public void Consume(Payload payload, Message<B> message)
            {
            }
        }


        class A
        {
            public string Value { get; set; }
        }

        class B
        {
            
        }
    }
}