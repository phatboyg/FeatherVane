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
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Vanes;
    using Web.Http;
    using Web.Http.Vanes;

    [TestFixture]
    public class Submitting_a_request :
        HttpServerTest
    {
        [Test]
        public void Should_get_a_200()
        {
            var requestUri = new Uri(ServerUri + "/hello");

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
            var requestUri = new Uri(ServerUri + "/hello");

            Stopwatch start = Stopwatch.StartNew();

            int threads = Environment.ProcessorCount*2;
            int iterations = 5000;

            Task[] tasks = Enumerable.Range(0, threads).Select(x => Task.Factory.StartNew(() =>
                {
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
                }, TaskCreationOptions.LongRunning)).ToArray();

            Task.WaitAll(tasks);

            start.Stop();

            Console.WriteLine("Elapsed Time: {0}ms", start.ElapsedMilliseconds);
            Console.WriteLine("Requests/second: {0}", (Stopwatch.Frequency/((decimal)start.ElapsedTicks/
                                                                            (threads*iterations))).ToString("F0"));
        }

        [Test, Explicit]
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

        protected override Vane<ConnectionContext> CreateMainVane()
        {
            return Vane.Connect(new Unhandled<ConnectionContext>(),
                new Profiler<ConnectionContext>(Console.Out, TimeSpan.FromMilliseconds(4)),
                new HelloFeatherVane(),
                new NotFoundFeatherVane());
        }

        class HelloFeatherVane :
            FeatherVane<ConnectionContext>
        {
            public void Build(Builder<ConnectionContext> builder,
                Payload<ConnectionContext> payload, Vane<ConnectionContext> next)
            {
                if (payload.Get<RequestContext>().Url.ToString().EndsWith("hello"))
                {
                    builder.Execute(() =>
                        {
                    ResponseContext response;
                            if (payload.TryGet(out response))
                            {
                                response.StatusCode = 200;
                                response.Write("Hello!");
                            }
                        });
                }

                next.Build(builder, payload);
            }
        }
    }
}