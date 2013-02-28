namespace FeatherVane.Tests
{
    using System;
    using System.Threading;
    using NUnit.Framework;
    using Support.CircuitBreakerFeather;


    [TestFixture]
    public class When_a_circuit_breaker_encounters_an_exception
    {
        [Test]
        public void Should_no_longer_call_the_vane()
        {
            int callCount = 0;
            var vane = VaneFactory.New<int>(x =>
                {
                    x.ConsoleLog(payload => string.Format("Executing: {0}", payload.Data));
                    x.CircuitBreaker(5);
                    x.Execute(payload =>
                        {
                            callCount++;
                            if(callCount <= 5)
                                throw new InvalidOperationException("Expected");
                        });
                });

            for (int i = 0; i < 5; i++)
            {
                var aggregateException = Assert.Throws<AggregateException>(() => vane.Execute(i));
                Assert.IsInstanceOf<InvalidOperationException>(aggregateException.InnerException);
            }

            var exception = Assert.Throws<AggregateException>(() => vane.Execute(5));
            Assert.IsInstanceOf<CircuitOpenException>(exception.InnerException);
            Assert.IsInstanceOf<InvalidOperationException>((exception.InnerException.InnerException));

            Thread.Sleep(500);

            vane.Execute(6);
        }
    }
}
