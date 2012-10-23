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
namespace FeatherVane.Tests.Actors
{
    using System;
    using FeatherVane.Actors.Payloads;
#if !NETFX_CORE
    using NUnit.Framework;
#else
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

#if !NETFX_CORE
    [TestFixture]
#else
    [TestClass]
#endif
    public class When_an_actor_is_built_on_top_of_feathervane
    {
#if !NETFX_CORE
        [Test]
#else
         [TestMethod]
#endif
        public void Should_be_appropriate()
        {
        }

        class ExitHandler :
            FeatherVane<Exit>
        {
            public Agenda<Exit> Plan(Planner<Exit> planner, Payload<Exit> payload, Vane<Exit> next)
            {
                throw new NotImplementedException();
            }
        }
    }
}