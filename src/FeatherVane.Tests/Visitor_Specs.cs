namespace FeatherVane.Tests
{
    using System;
    using System.IO;
    using Feathers;
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

            var typeRouter = new TypeRouterFeather<TestSubject>(x => typeof(ISubjectA));
            typeRouter.Add(VaneFactory.Success<ISubjectA>(), x => x);
            typeRouter.Add(VaneFactory.Success<ISubjectB>(), x => x);

            Vane<TestSubject> vane = VaneFactory.Success(new LogFeather<TestSubject>(sw, x => ""),
                new WireTapFeather<TestSubject>(wired),
                new ProfilerFeather<TestSubject>(sw, TimeSpan.FromMilliseconds(2)),
                new TransactionFeather<TestSubject>(),
                new Test(),
                typeRouter);

            var visitor = new StringVaneVisitor();
            visitor.Visit(vane);

            Console.WriteLine(visitor);
        }
    }
}
