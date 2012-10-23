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
    using NUnit.Framework;
#else
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

#if !NETFX_CORE
    [TestFixture]
#else
    [TestClass]
#endif
    public class When_building_a_plan
    {
#if !NETFX_CORE
        [Test]
#else
         [TestMethod]
#endif
        public void Should_create_the_plan_for_fail()
        {
            var fail = new TestFail();
            Vane<TestSubject> vane = Vane.Connect(fail);

#if !NETFX_CORE
            var exception = Assert.Throws<AgendaExecutionException>(() => vane.Execute(_testSubject));
#else
            var exception = Assert.ThrowsException<AgendaExecutionException>(() => vane.Execute(_testSubject));
#endif

            Assert.AreEqual(0, exception.InnerExceptionCount);
        }

#if !NETFX_CORE
        [Test]
#else
         [TestMethod]
#endif
        public void Should_create_the_plan_for_success()
        {
            var success = new TestSuccess();

            Vane<TestSubject> vane = Vane.Connect(success);
            vane.Execute(_testSubject);

            Assert.IsTrue(success.AssignCalled);
        }

#if !NETFX_CORE
        [Test]
#else
         [TestMethod]
#endif
        public void Should_execute_and_compensate_on_fail()
        {
            var fail = new TestFail();
            var log = new TestVane();
            Vane<TestSubject> vane = Vane.Connect(fail, log);

#if !NETFX_CORE
            var exception = Assert.Throws<AgendaExecutionException>(() => vane.Execute(_testSubject));
#else
            var exception = Assert.ThrowsException<AgendaExecutionException>(() => vane.Execute(_testSubject));
#endif

            Assert.AreEqual(0, exception.InnerExceptionCount);

            Assert.IsTrue(fail.AssignCalled);
            Assert.IsTrue(fail.ExecuteCalled);
            Assert.IsFalse(fail.CompensateCalled);

            Assert.IsTrue(log.AssignCalled);
            Assert.IsTrue(log.ExecuteCalled);
            Assert.IsTrue(log.CompensateCalled);
        }

#if !NETFX_CORE
        [Test]
#else
         [TestMethod]
#endif
        public void Should_execute_two_and_compensate_on_fail()
        {
            var fail = new TestFail();
            var log = new TestVane();
            var log2 = new TestVane();
            Vane<TestSubject> vane = Vane.Connect(fail, log, log2);

#if !NETFX_CORE
            var exception = Assert.Throws<AgendaExecutionException>(() => vane.Execute(_testSubject));
#else
            var exception = Assert.ThrowsException<AgendaExecutionException>(() => vane.Execute(_testSubject));
#endif

            Assert.AreEqual(0, exception.InnerExceptionCount);

            Assert.IsTrue(fail.AssignCalled);
            Assert.IsTrue(fail.ExecuteCalled);
            Assert.IsFalse(fail.CompensateCalled);

            Assert.IsTrue(log.AssignCalled);
            Assert.IsTrue(log.ExecuteCalled);
            Assert.IsTrue(log.CompensateCalled);

            Assert.IsTrue(log2.AssignCalled);
            Assert.IsTrue(log2.ExecuteCalled);
            Assert.IsTrue(log2.CompensateCalled);
        }

#if !NETFX_CORE
        [Test]
#else
         [TestMethod]
#endif
        public void Should_execute_both_vanes()
        {
            var success = new TestSuccess();
            var log = new TestVane();
            Vane<TestSubject> vane = Vane.Connect(success, log);

            vane.Execute(_testSubject);

            Assert.IsTrue(success.AssignCalled);

            Assert.IsTrue(log.AssignCalled);
            Assert.IsTrue(log.ExecuteCalled);
            Assert.IsFalse(log.CompensateCalled);
        }

#if !NETFX_CORE
        [Test]
#else
         [TestMethod]
#endif
        public void Should_execute_three_vanes()
        {
            var success = new TestSuccess();
            var log = new TestVane();
            var log2 = new TestVane();
            Vane<TestSubject> vane = Vane.Connect(success, log, log2);

            vane.Execute(_testSubject);

            Assert.IsTrue(success.AssignCalled);

            Assert.IsTrue(log.AssignCalled);
            Assert.IsTrue(log.ExecuteCalled);
            Assert.IsFalse(log.CompensateCalled);

            Assert.IsTrue(log2.AssignCalled);
            Assert.IsTrue(log2.ExecuteCalled);
            Assert.IsFalse(log2.CompensateCalled);
        }

        TestSubject _testSubject = new TestSubject {Street = "123"};
    }
}