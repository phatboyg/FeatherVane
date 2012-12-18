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

            Vane<TestSubject> wired = VaneBuilder.Success<TestSubject>();

            var typeRouter = new TypeRouter<TestSubject>(x => typeof(ISubjectA));
            typeRouter.Add(VaneBuilder.Success<ISubjectA>(), x => x);
            typeRouter.Add(VaneBuilder.Success<ISubjectB>(), x => x);

            Vane<TestSubject> vane = VaneBuilder.Success(new Logger<TestSubject>(sw, x => ""),
                new WireTap<TestSubject>(wired),
                new Profiler<TestSubject>(sw, TimeSpan.FromMilliseconds(2)),
                new Transaction<TestSubject>(),
                new TestVane(),
                typeRouter);

            var visitor = new StringVaneVisitor();
            visitor.Visit(vane);

            Console.WriteLine(visitor);
        }
    }
}
