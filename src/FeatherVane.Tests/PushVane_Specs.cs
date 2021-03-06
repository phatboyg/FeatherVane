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
namespace FeatherVane.Tests
{
    using System;
    using NUnit.Framework;
    using Taskell;
    using Vanes;

    [TestFixture]
    public class When_intercepting_a_vane
    {
        [Test]
        public void Should_be_able_to_push_in_front()
        {
            Vane<string> original = VaneFactory.Connect(new SuccessVane<string>(),
                new Original());

            original.Execute("First");

            Vane<string> decorated = original.Push(new Decorator());

            decorated.Execute("Second");
        }

        class Original :
            Feather<string>
        {
            public void Compose(Composer composer, Payload<string> payload, Vane<string> next)
            {
                Console.WriteLine("Original: {0}", payload.Data);

                next.Compose(composer, payload);
            }
        }

        class Decorator :
            Feather<string>
        {
            public void Compose(Composer composer, Payload<string> payload, Vane<string> next)
            {
                Console.WriteLine("Decorator: {0}", payload.Data);

                next.Compose(composer, payload);
            }
        }
    }
}