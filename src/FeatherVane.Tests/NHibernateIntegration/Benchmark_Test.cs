namespace FeatherVane.Tests.NHibernateIntegration
{
    using Benchmarks;
    using NUnit.Framework;


    [TestFixture]
    public class Benchmark_Test
    {
        [Test]
        public void FirstTestName()
        {
            var throughput = new NHibernateThroughput();

            var subject = new Subject {Id = 1};

            throughput.Execute(subject);
        }
    }
}
