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
namespace FeatherVane.Tests.HttpTests
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using NUnit.Framework;
    using Vanes;
    using Web.Http;
    using Web.Http.Vanes;

    [TestFixture]
    public class Submitting_a_request :
        HttpServerTest
    {
        [Test]
        public void Should_get_not_found()
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(ServerUri);
            HttpWebResponse _webResponse;
            try
            {
                _webResponse = (HttpWebResponse)webRequest.GetResponse();
            }
            catch (WebException ex)
            {
                _webResponse = (HttpWebResponse)ex.Response;
            }
            using (_webResponse)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, _webResponse.StatusCode);

                _webResponse.Close();
            }
        }

        [Test]
        public void Should_get_a_200()
        {
            Uri requestUri = new Uri(ServerUri.ToString() + "/hello");

            var webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            HttpWebResponse _webResponse;
            try
            {
                _webResponse = (HttpWebResponse)webRequest.GetResponse();
            }
            catch (WebException ex)
            {
                _webResponse = (HttpWebResponse)ex.Response;
            }
            using (_webResponse)
            {
                Assert.AreEqual(HttpStatusCode.OK, _webResponse.StatusCode);

                _webResponse.Close();
            }
        }

        [Test]
        public void Should_get_a_whole_lot_of_200s()
        {
            Uri requestUri = new Uri(ServerUri.ToString() + "/hello");

            Stopwatch start = Stopwatch.StartNew();

            int iterations = 1000;
            for (int i = 0; i < iterations; i++)
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
                HttpWebResponse _webResponse;
                try
                {
                    _webResponse = (HttpWebResponse)webRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    _webResponse = (HttpWebResponse)ex.Response;
                }
                using (_webResponse)
                {
                    Assert.AreEqual(HttpStatusCode.OK, _webResponse.StatusCode);

                    _webResponse.Close();
                }
            }

            start.Stop();

            Console.WriteLine("Elapsed Time: {0}ms", start.ElapsedMilliseconds);
            Console.WriteLine("Requests/second: {0}", Stopwatch.Frequency/((decimal)start.ElapsedTicks/iterations));
        }

        protected override NextVane<ConnectionContext> CreateMainVane()
        {
            return NextVane.Connect(new UnhandledVane<ConnectionContext>(),
                new Profiler<ConnectionContext>(Console.Out, TimeSpan.FromMilliseconds(2)),
                new HelloVane(),
                new NotFoundVane());
        }

        class HelloVane :
            Vane<ConnectionContext>
        {
            public VaneHandler<ConnectionContext> GetHandler(ConnectionContext context, NextVane<ConnectionContext> next)
            {
                if (context.Request.Url.ToString().EndsWith("hello"))
                    return new HelloVaneHandler();

                return next.GetHandler(context);
            }

            class HelloVaneHandler : VaneHandler<ConnectionContext>
            {
                public void Handle(ConnectionContext context)
                {
                    context.Response.StatusCode = 200;
                    context.Response.Write("Hello!");
                }
            }
        }
    }
}