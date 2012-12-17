﻿// Copyright 2012-2012 Chris Patterson
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
namespace FeatherVane.Tests.HttpTests
{
    using System;
    using NUnit.Framework;
    using Vanes;
    using Web.Http;
    using Web.Http.Vanes;

    [TestFixture]
    public class HttpServerTest
    {
        [Test]
        public void Should_startup()
        {
        }

        HttpServer _server;

        [TestFixtureSetUp]
        public void Setup_http_server()
        {
            Vane<ConnectionContext> vane = CreateMainVane();

            ServerUri = new Uri("http://localhost:8008/FeatherVaneTests");
            _server = new HttpServer(ServerUri, vane);
            _server.Start();
        }

        [TestFixtureTearDown]
        public void Teardown_http_server()
        {
            _server.Stop();
            Console.WriteLine("Maximum Concurrent Connections: {0}", _server.MaxConnections);
        }

        protected Uri ServerUri { get; private set; }

        protected virtual Vane<ConnectionContext> CreateMainVane()
        {
            return Vane.Connect(new Unhandled<ConnectionContext>(),
                new Profiler<ConnectionContext>(Console.Out, TimeSpan.FromMilliseconds(4)),
                new Logger<ConnectionContext>(Console.Error, x => x.Get<RequestContext>().Url.ToString()),
                new NotFoundFeatherVane());
        }
    }
}