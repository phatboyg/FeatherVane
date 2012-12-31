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
    using NUnit.Framework;
    using Vanes;


    [TestFixture]
    public class Using_the_fluent_configuration_syntax
    {
        [Test]
        public void Should_default_to_a_successful_empty_vane()
        {
            Vane<Message> vane = VaneFactory.New<Message>(x => { });

            Assert.IsInstanceOf<Success<Message>>(vane);
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

            Assert.IsInstanceOf<ExecuteTask<Message>>(nextVane.FeatherVane);
            Assert.IsInstanceOf<Success<Message>>(nextVane.Next);
        }

        [Test]
        public void Should_include_an_execute_vane()
        {
            Vane<Message> vane = VaneFactory.New<Message>(x => x.Execute(message => { }));

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<Execute<Message>>(nextVane.FeatherVane);
            Assert.IsInstanceOf<Success<Message>>(nextVane.Next);
        }

        [Test]
        public void Should_include_a_logger_vane()
        {
            Vane<Message> vane =
                VaneFactory.New<Message>(x => x.Logger(v => v.SetOutput(Console.Out).SetFormat(d => d.GetType().Name)));

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<Logger<Message>>(nextVane.FeatherVane);
            Assert.IsInstanceOf<Success<Message>>(nextVane.Next);
        }

        [Test]
        public void Should_include_a_profiler_vane()
        {
            Vane<Message> vane =
                VaneFactory.New<Message>(x => x.Profiler(v => v.SetOutput(Console.Out).Threshold(TimeSpan.Zero)));

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<Profiler<Message>>(nextVane.FeatherVane);
            Assert.IsInstanceOf<Success<Message>>(nextVane.Next);
        }

        [Test]
        public void Should_include_a_fanout_vane()
        {
            Vane<Message> vane = VaneFactory.New<Message>(x =>
                {
                    x.Fanout(fx =>
                        {
                            fx.Logger(v => v.SetOutput(Console.Out).SetFormat(k => ""));
                            fx.Profiler(v => v.SetOutput(Console.Out));
                        });
                });

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<Fanout<Message>>(nextVane.FeatherVane);
            Assert.IsInstanceOf<Success<Message>>(nextVane.Next);

            var fanout = nextVane.FeatherVane as Fanout<Message>;
            Assert.IsNotNull(fanout);

            Assert.AreEqual(2, fanout.Count);
        }

        [Test]
        public void Should_include_a_splice_vane()
        {
            SourceVane<A> sourceVane = null;
            Vane<Message> vane = VaneFactory.New<Message>(x =>
                {
                    x.Splice(s => s.Source(sourceVane, fx =>
                        {
                            fx.Logger(v => v.SetOutput(Console.Out).SetFormat(k => ""));
                            fx.Profiler(v => v.SetOutput(Console.Out));
                        }));
                });

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<Splice<Message, A>>(nextVane.FeatherVane);
            Assert.IsInstanceOf<Success<Message>>(nextVane.Next);
        }

        [Test]
        public void Should_include_a_splice_and_source_vane()
        {
            Vane<Message> vane = VaneFactory.New<Message>(x =>
                {
                    x.Splice(s => s.Factory(() => new A(), fx =>
                        {
                            fx.Logger(v => v.SetOutput(Console.Out).SetFormat(k => ""));
                            fx.Profiler(v => v.SetOutput(Console.Out));
                        }, sx =>
                            {
                                // output
                                sx.Execute(message => { });
                            }));
                });

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<Splice<Message, A>>(nextVane.FeatherVane);
            Assert.IsInstanceOf<Success<Message>>(nextVane.Next);
        }

        [Test]
        public void Should_include_ability_to_create_a_source_vane()
        {
            SourceVane<A> sourceVane = SourceVaneFactory.New<A>(x =>
                {
                    // simple factory method
                    x.Factory(() => new A());
                });

            Assert.IsInstanceOf<Factory<A>>(sourceVane);

        }


        class A
        {
        }
    }
}