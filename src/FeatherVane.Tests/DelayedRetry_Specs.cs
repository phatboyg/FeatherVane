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
namespace FeatherVane.Tests
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using SourceVanes;
    using Taskell;


    [TestFixture]
    public class Using_a_delayed_retry_source_vane
    {
        [Test]
        public void Should_not_retry_on_vane_failure()
        {
            Vane<string> vane = VaneFactory.New<string>(x =>
                {
                    x.Splice(s => s.Source<string>(
                        source =>
                        source.UseSourceVane(() => new DelayedRetrySourceVane<string>(new FirstFailSourceVane())),
                        output => output.Execute(v =>
                            {
                                Console.WriteLine("Executing final vane");
                                throw new NotImplementedException("Well this is a real pickle isn't it");
                            })));
                });


            var aggregateException = Assert.Throws<AggregateException>(() => vane.Execute("Hello"));

            Assert.IsInstanceOf<NotImplementedException>(aggregateException.InnerException);
        }

        [Test]
        public void Should_retry_on_failure_without_failing_the_vane()
        {
            bool success = false;

            Vane<string> vane = VaneFactory.New<string>(x =>
                {
                    x.Splice(s => s.Source<string>(
                        source =>
                        source.UseSourceVane(() => new DelayedRetrySourceVane<string>(new FirstFailSourceVane())),
                        output => output.Execute(v =>
                            {
                                Console.WriteLine("Executing final vane");
                                success = true;
                            })));
                });


            Task task = vane.ExecuteAsync("Hello");

            task.Wait();

            Console.WriteLine("Task Status: {0}", task.Status);

            Assert.IsTrue(success);
        }

        [Test]
        public void Should_only_retry_up_to_timeout_limit()
        {
            Vane<string> vane = VaneFactory.New<string>(x =>
                {
                    x.Splice(s => s.Source<string>(
                        source =>
                        source.UseSourceVane(() => new DelayedRetrySourceVane<string>(new FirstFailSourceVane(), new[]{0})),
                        output => output.Execute(v =>
                            {
                                Console.WriteLine("Executing final vane");
                            })));
                });

            var aggregateException = Assert.Throws<AggregateException>(() => vane.Execute("Hello"));

            Assert.IsInstanceOf<InvalidOperationException>(aggregateException.InnerException);
        }

        [Test]
        public void Should_not_retry_if_no_timeouts()
        {
            Vane<string> vane = VaneFactory.New<string>(x =>
                {
                    x.Splice(s => s.Source<string>(
                        source =>
                        source.UseSourceVane(() => new DelayedRetrySourceVane<string>(new FirstFailSourceVane(), new int[]{})),
                        output => output.Execute(v =>
                            {
                                Console.WriteLine("Executing final vane");
                            })));
                });

            var aggregateException = Assert.Throws<AggregateException>(() => vane.Execute("Hello"));

            Assert.IsInstanceOf<InvalidOperationException>(aggregateException.InnerException);
        }
    }


    class FirstFailSourceVane :
        SourceVane<string>
    {
        int _count;

        public void Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, string>> next)
        {
            composer.Execute(() =>
                {
                    _count++;
                    if (_count <= 2)
                    {
                        Console.WriteLine("Throwing exception  pass {0}", _count);
                        throw new InvalidOperationException("This is expected on the first call");
                    }

                    Payload<Tuple<TPayload, string>> nextPayload = payload.MergeRight("World");

                    return composer.ComposeTask(next, nextPayload);
                });
        }
    }
}