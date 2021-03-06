﻿// Copyright 2012-2013 Chris Patterson
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
    using NUnit.Framework;


    [TestFixture]
    public class When_the_main_vane_fails
    {
        [Test]
        public void Should_complete_the_rescue_vane()
        {
            var fail = new TestFail();
            var middle = new Test();

            var success = new TestSuccess();

            Vane<TestSubject> vane = VaneFactory.New(() => fail, x =>
                {
                    x.Rescue(() => success, r => { });
                    x.Feather(() => middle);
                });

            vane.Execute(_testSubject);

            Assert.IsTrue(middle.AssignCalled);
            Assert.IsTrue(middle.ExecuteCalled);
            Assert.IsTrue(middle.CompensateCalled);

            Assert.IsTrue(success.AssignCalled);

            Assert.IsTrue(fail.AssignCalled);
            Assert.IsTrue(fail.ExecuteCalled);
            Assert.IsTrue(fail.CompensateCalled);
        }

        TestSubject _testSubject = new TestSubject {Street = "123"};
    }
}