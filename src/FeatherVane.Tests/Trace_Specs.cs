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
namespace FeatherVane.Tests
{
    using System;
    using NUnit.Framework;
    using Vanes;

    [TestFixture]
    public class Using_a_trace_vane
    {
        [Test]
        public void Should_be_called_during_the_final_execution_state()
        {
            int expected = 27;
            int? called = null;

            NextVane<int> endVane = new SuccessVane<int>();


            Vane<int> traceVane = new TraceLogger<int>(x =>
                {
                    called = x;
                    return x.ToString();
                });

            Vane<int> profiler = new Profiler<int>(TimeSpan.FromMilliseconds(2));

            NextVane<int> vane = profiler.WithNext(traceVane.WithNext(endVane));

            VaneHandler<int> handler = vane.GetHandler(expected);

            Assert.IsFalse(called.HasValue);

            handler.Handle(expected);

            Assert.IsTrue(called.HasValue);
            Assert.AreEqual(expected, called.Value);
        }
    }
}