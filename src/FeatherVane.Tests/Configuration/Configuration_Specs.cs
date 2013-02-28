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
namespace FeatherVane.Tests.Configuration
{
    using System;
    using System.Threading.Tasks;
    using FeatherVane.Messaging;
    using FeatherVane.Messaging.Payloads;
    using Feathers;
    using NUnit.Framework;
    using SourceVanes;
    using Vanes;


    [TestFixture]
    public class Using_the_fluent_configuration_syntax
    {
        [Test]
        public void Should_default_to_a_successful_empty_vane()
        {
            Vane<Message> vane = VaneFactory.New<Message>(x => { });

            Assert.IsInstanceOf<SuccessVane<Message>>(vane);
        }

        [Test]
        public void Should_include_an_execute_task_vane()
        {
            Vane<Message> vane = VaneFactory.New<Message>(x => x.ExecuteTask(message =>
                {
                    Task task = Task.Factory.StartNew(() => { });

                    return task;
                }));

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<ExecuteTaskFeather<Message>>(nextVane.Feather);
            Assert.IsInstanceOf<SuccessVane<Message>>(nextVane.Next);
        }

        [Test]
        public void Should_include_an_execute_vane()
        {
            Vane<Message> vane = VaneFactory.New<Message>(x => x.Execute(message => { }));

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<ExecuteFeather<Message>>(nextVane.Feather);
            Assert.IsInstanceOf<SuccessVane<Message>>(nextVane.Next);
        }

        [Test]
        public void Should_include_a_logger_vane()
        {
            Vane<Message> vane =
                VaneFactory.New<Message>(x => x.Log(v => v.SetOutput(Console.Out).SetFormat(d => d.GetType().Name)));

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<LogFeather<Message>>(nextVane.Feather);
            Assert.IsInstanceOf<SuccessVane<Message>>(nextVane.Next);
        }

        [Test]
        public void Should_include_a_profiler_vane()
        {
            Vane<Message> vane =
                VaneFactory.New<Message>(x => x.Profiler(v => v.SetOutput(Console.Out).Threshold(TimeSpan.Zero)));

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<ProfilerFeather<Message>>(nextVane.Feather);
            Assert.IsInstanceOf<SuccessVane<Message>>(nextVane.Next);
        }

        [Test]
        public void Should_include_a_fanout_vane()
        {
            Vane<Message> vane = VaneFactory.New<Message>(x =>
                {
                    x.Fanout(fx =>
                        {
                            fx.Log(v => v.SetOutput(Console.Out).SetFormat(k => ""));
                            fx.Profiler(v => v.SetOutput(Console.Out));
                        });
                });

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<FanoutFeather<Message>>(nextVane.Feather);
            Assert.IsInstanceOf<SuccessVane<Message>>(nextVane.Next);

            var fanout = nextVane.Feather as FanoutFeather<Message>;
            Assert.IsNotNull(fanout);

            Assert.AreEqual(2, fanout.Count);
        }

        [Test]
        public void Should_include_a_splice_vane()
        {
            SourceVane<A> sourceVane = null;
            Vane<Message> vane = VaneFactory.New<Message>(x =>
                {
                    x.Splice(s => s.Source<A>(sx => sx.UseSourceVane(() => sourceVane), fx =>
                        {
                            fx.Log(v => v.SetOutput(Console.Out).SetFormat(k => ""));
                            fx.Profiler(v => v.SetOutput(Console.Out));
                        }));
                });

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<SpliceFeather<Message, A>>(nextVane.Feather);
            Assert.IsInstanceOf<SuccessVane<Message>>(nextVane.Next);
        }

        [Test]
        public void Should_include_a_splice_and_source_vane()
        {
            Vane<Message> vane = VaneFactory.New<Message>(x =>
                {
                    x.Splice(s => s.Source<A>(sx =>
                        {
                            sx.Factory(() => new A());
                            sx.Log(v => v.SetOutput(Console.Out).SetFormat(k => ""));
                            sx.Profiler(v => v.SetOutput(Console.Out));
                        }, vx =>
                            {
                                // output
                                vx.Execute(message => { });
                            }));
                });

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<SpliceFeather<Message, A>>(nextVane.Feather);
            Assert.IsInstanceOf<SuccessVane<Message>>(nextVane.Next);
        }

        [Test]
        public void Should_include_ability_to_create_a_source_vane()
        {
            SourceVane<A> sourceVane = SourceVaneFactory.New<A>(x =>
                {
                    // simple factory method
                    x.Factory(() => new A());
                });

            Assert.IsInstanceOf<FactorySourceVane<A>>(sourceVane);

        }


        class A
        {
        }
    }
}