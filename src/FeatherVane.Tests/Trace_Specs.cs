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

            NextVane<int> endVane = new EndVane<int>();


            Vane<int> traceVane = new TraceLogger<int>(x =>
                {
                    called = x;
                    return x.ToString();
                });

            Vane<int> profiler = new Profiler<int>();

            NextVane<int> vane = profiler.WithNext(traceVane.WithNext(endVane));

            Action<int> action = vane.Handle(expected);

            Assert.IsFalse(called.HasValue);

            action(expected);

            Assert.IsTrue(called.HasValue);
            Assert.AreEqual(expected, called.Value);
        }
    }
}