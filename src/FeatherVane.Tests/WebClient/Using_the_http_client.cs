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
namespace FeatherVane.Tests.WebClient
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;


    [TestFixture, Explicit]
    public class Using_the_http_client
    {
        [Test]
        public void Should_be_easy_and_painless()
        {
            Vane<RequestWeather> vane = VaneFactory.New<RequestWeather>(x =>
                {
                    x.Splice(o => o.Source<WeatherConditions>(svc => svc.UseSourceVane(() => new WeatherVane()),
                        vc =>
                            {
                                vc.Execute(payload =>
                                    {
                                        Console.WriteLine("Temp: {0}F, {1}", payload.Data.Item2.Temperature,
                                            payload.Data.Item2.Description);
                                    });
                            }));
                });

            vane.Execute(new RequestWeather {WeatherId = "USCA0350"});
        }


        class WeatherConditions
        {
            public string Temperature { get; set; }
            public string Description { get; set; }
        }


        class RequestWeather
        {
            public string WeatherId { get; set; }
        }


        /// <summary>
        /// Wraps a factory method into a SourceVane, allowing objects to be created within the
        /// scope of an execution with full lifecycle management.
        /// </summary>
        /// <typeparam name="T">The source type for the splice</typeparam>
        public class WeatherVane :
            SourceVane<WeatherConditions>
        {
            void SourceVane<WeatherConditions>.Compose<TPayload>(Composer composer, Payload<TPayload> payload,
                Vane<Tuple<TPayload, WeatherConditions>> next)
            {
                HttpClient httpClient = null;
                composer.Execute(() =>
                    {
                        var request = payload.Data as RequestWeather;
                        if (request == null)
                            throw new ArgumentException("Invalid payload type");

                        var sourceUri = new UriBuilder(@"http://query.yahooapis.com/v1/public/yql");
                        sourceUri.Query = @"q=" + @"select item from weather.forecast where location="""
                                          + request.WeatherId + @""""
                                          + @"&format=json";

                        httpClient = new HttpClient();
                        return httpClient.GetAsync(sourceUri.Uri)
                                         .ContinueWith(task =>
                                             {
                                                 task.Result.EnsureSuccessStatusCode();
                                                 return task.Result.Content.ReadAsAsync<JContainer>();
                                             },
                                             composer.CancellationToken,
                                             TaskContinuationOptions.OnlyOnRanToCompletion,
                                             TaskScheduler.Current)
                                         .FastUnwrap()
                                         .ContinueWith(task =>
                                             {
                                                 JToken conditions =
                                                     task.Result["query"]["results"]["channel"]["item"]["condition"];

                                                 var temp = (string)conditions["temp"];
                                                 var text = (string)conditions["text"];

                                                 var data = new WeatherConditions
                                                     {
                                                         Temperature = temp,
                                                         Description = text
                                                     };

                                                 Payload<Tuple<TPayload, WeatherConditions>> nextPayload =
                                                     payload.CreateProxy(Tuple.Create(payload.Data, data));

                                                 return TaskComposer.Compose(next, nextPayload,
                                                     composer.CancellationToken);
                                             },
                                             composer.CancellationToken,
                                             TaskContinuationOptions.OnlyOnRanToCompletion,
                                             TaskScheduler.Current)
                                         .FastUnwrap();
                    });

                composer.Finally(() =>
                    {
                        if (httpClient != null)
                            httpClient.Dispose();
                    });
            }
        }
    }
}