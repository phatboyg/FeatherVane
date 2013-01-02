namespace FeatherVane.Tests
{
    using System;
    using System.IO;
    using NUnit.Framework;
    using Vanes;
    using Visualization;

    [TestFixture]
    public class Visiting_a_set_of_vanes
    {
        [Test]
        public void Should_display_the_types_of_all_the_vanes()
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            Vane<TestSubject> wired = VaneFactory.Success<TestSubject>();

            var typeRouter = new TypeRouterVane<TestSubject>(x => typeof(ISubjectA));
            typeRouter.Add(VaneFactory.Success<ISubjectA>(), x => x);
            typeRouter.Add(VaneFactory.Success<ISubjectB>(), x => x);

            Vane<TestSubject> vane = VaneFactory.Success(new LogVane<TestSubject>(sw, x => ""),
                new WireTapVane<TestSubject>(wired),
                new ProfilerVane<TestSubject>(sw, TimeSpan.FromMilliseconds(2)),
                new TransactionVane<TestSubject>(),
                new TestVane(),
                typeRouter);

            var visitor = new StringVaneVisitor();
            visitor.Visit(vane);

            Console.WriteLine(visitor);
        }
    }
}
