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
#if !NETFX_CORE
    using System;
    using NUnit.Framework;
#else
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif
    using Vanes;

#if !NETFX_CORE
    [TestFixture]
#else
    [TestClass]
#endif
    public class When_intercepting_a_vane
    {
#if !NETFX_CORE
        [Test]
#else
         [TestMethod]
#endif
        public void Should_be_able_to_push_in_front()
        {
            Vane<string> original = Vane.Connect(new Success<string>(),
                new Original());

            original.Execute("First");

            Vane<string> decorated = original.Push(new Decorator());

            decorated.Execute("Second");
        }

        class Original :
            FeatherVane<string>
        {
            public Agenda<string> Plan(Planner<string> planner, Payload<string> payload, Vane<string> next)
            {
#if !NETFX_CORE
                Console.WriteLine("Original: {0}", payload.Data);
#else
                Debug.WriteLine("Original: {0}", payload.Data);
#endif
                return next.Plan(planner, payload);
            }
        }

        class Decorator :
            FeatherVane<string>
        {
            public Agenda<string> Plan(Planner<string> planner, Payload<string> payload, Vane<string> next)
            {
#if !NETFX_CORE
                Console.WriteLine("Decorator: {0}", payload.Data);
#else
                Debug.WriteLine("Decorator: {0}", payload.Data);
#endif
                return next.Plan(planner, payload);
            }
        }
    }
}