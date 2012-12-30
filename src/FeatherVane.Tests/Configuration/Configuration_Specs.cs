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
            Vane<Message> vane = VaneFactory.New<Message>(x => x.Execute(message =>
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
                VaneFactory.New<Message>(x => x.Logger(v => v.SetOutput(Console.Out).SetFormatter(d => d.GetType().Name)));

            var nextVane = vane as NextVane<Message>;
            Assert.IsNotNull(nextVane);

            Assert.IsInstanceOf<Logger<Message>>(nextVane.FeatherVane);
            Assert.IsInstanceOf<Success<Message>>(nextVane.Next);
        }
    }
}