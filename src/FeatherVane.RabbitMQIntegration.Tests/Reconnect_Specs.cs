// Copyright 2012-2013 Chris Patterson
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
namespace FeatherVane.RabbitMQIntegration.Tests
{
    using System;
    using ConnectionManagement;
    using NUnit.Framework;
    using RabbitMQ.Client;
    using SourceVanes;
    using Vanes;


    [TestFixture]
    public class When_forcing_a_reconnect
    {
        [Test]
        public void Should_be_able_to_get_a_model()
        {
            bool open = false;

            Vane<IContext> vane =
                VaneFactory.New<IContext>(
                    x =>
                        {
                            x.Splice(s => s.Source<IModel>(m => m.UseSourceVane(() => _modelVane),
                                splice => splice.Execute(payload => { open = payload.Data.Item2.IsOpen; })));
                        });

            var context = new Context();
            vane.Execute(context);
            vane.Execute(context);
            vane.Execute(context);
            vane.Execute(context);

            Assert.IsTrue(open);
        }

        [Test]
        public void Should_create_a_new_model_on_exception()
        {
            Vane<IContext> vane =
                VaneFactory.New<IContext>(
                    x =>
                        {
                            x.Splice(s => s.Source<IModel>(m => m.UseSourceVane(() => _modelVane),
                                splice =>
                                splice.Execute(
                                    payload =>
                                        { throw new InvalidOperationException("Well, expected but still not fun."); })));
                        });

            var context = new Context();

            var exception = Assert.Throws<AggregateException>(() => vane.Execute(context));
            Assert.IsInstanceOf<InvalidOperationException>(exception.InnerException);
            Assert.Throws<AggregateException>(() => vane.Execute(context));
        }

        ConnectionFactory _connectionFactory;
        SourceVane<IModel> _modelVane;
        PoolSourceVane<IConnection> _poolSourceVane;

        [TestFixtureSetUp]
        public void Setup()
        {
            _connectionFactory = new ConnectionFactory
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest",
                    VirtualHost = "/"
                };

            SourceVane<IConnection> connectVane = SourceVaneFactory.New<IConnection>(x =>
                {
                    x.UseSourceVane(() => new ConnectVane(_connectionFactory));
                    x.ConsoleLog(payload => string.Format("Connection Created: {0}",
                        payload.Data.ServerProperties.FormatKeyValue(": ", Environment.NewLine)));
                });

            Vane<IConnection> disconnectVane = VaneFactory.New(() => new DisconnectVane(),
                x => x.ConsoleLog(payload => "Closing connection"));

            _poolSourceVane = new PoolSourceVane<IConnection>(connectVane, disconnectVane);
            SourceVane<IConnection> poolVane = SourceVaneFactory.New<IConnection>(x =>
                {
                    x.UseExistingSourceVane(_poolSourceVane);
                    x.ConsoleLog(payload => string.Format("Using Connection"));
                });

            _modelVane = SourceVaneFactory.New<IModel>(x => x.UseSourceVane(() => new ModelSourceVane(poolVane)));
        }

        [TestFixtureTearDown]
        public void Teardown()
        {
            _poolSourceVane.Dispose();
        }


        interface IContext
        {
        }


        class Context : IContext
        {
        }
    }
}