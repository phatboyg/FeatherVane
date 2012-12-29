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
namespace FeatherVane.Tests.NHibernateIntegration
{
    using System;
    using System.IO;
    using FeatherVane.NHibernateIntegration.Vanes;
    using NHibernate;
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;
    using NUnit.Framework;
    using Vanes;
    using Visualizer;


    [TestFixture]
    public class Loading_an_object_for_a_payload :
        SQLiteTestFixture
    {
        [Test]
        public void Should_pass_if_it_exists()
        {
            Console.WriteLine("Running test with id {0}", _id);

            var executeVane = new Execute<Tuple<A, Subject>>(x => { });
            Vane<Tuple<A, Subject>> finalVane = VaneFactory.Success(executeVane);

            var id = new Id<A, int>(x => x.Id);
            var factory = new Factory<Subject>(() => new Subject());
            var loadVane = new Load<Subject, int>(SessionFactory, id, factory);

            var loggerVane = new Logger<Subject>(Console.Out, x => "Loaded subject: " + x.Data.Id);

            SourceVane<Subject> sourceVane = VaneFactory.Source(loadVane, loggerVane);
            var spliceVane = new Splice<A, Subject>(finalVane, sourceVane);


            var fanOutVane = new Fanout<A>(new FeatherVane<A>[] {spliceVane});

            var profilerVane = new Profiler<A>(Console.Out, TimeSpan.FromMilliseconds(1));

            Vane<A> vane = VaneFactory.Success(profilerVane, fanOutVane);

            vane.RenderGraphToFile(new FileInfo("NHibernateTestGraph.png"));

            var a = new A {Id = _id};
            vane.Execute(a);
            vane.Execute(a);
            var a2 = new A {Id = 47};
            vane.Execute(a2);
        }

        [Test]
        public void Should_throw_if_an_object_is_not_found()
        {
            var executeVane = new Execute<Tuple<A, Subject>>(x => { });
            Vane<Tuple<A, Subject>> finalVane = VaneFactory.Success(executeVane);

            var id = new Id<A, int>(x => x.Id);
            var factory = new Missing<Subject>();
            var loadVane = new Load<Subject, int>(SessionFactory, id, factory);

            SourceVane<Subject> sourceVane = VaneFactory.Source(loadVane);
            var spliceVane = new Splice<A, Subject>(finalVane, sourceVane);

            Vane<A> vane = VaneFactory.Success(spliceVane);

            var a = new A {Id = 47};
            var aggregateException = Assert.Throws<AggregateException>(() => vane.Execute(a));

            Assert.IsInstanceOf<FeatherVane.ObjectNotFoundException>(aggregateException.InnerException);
        }

        int _id;

        public Loading_an_object_for_a_payload()
        {
            Map(typeof(SubjectMap));
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            var subject = new Subject {Name = "Joe", Value = "Cool"};

            using (ISession session = SessionFactory.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                session.Save(subject);
                tx.Commit();
            }

            _id = subject.Id;
        }


        class A
        {
            public int Id { get; set; }
        }


        public class Subject
        {
            public int Id { get; private set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }


        public class SubjectMap :
            ClassMapping<Subject>
        {
            public SubjectMap()
            {
                Table("Subject");
                Lazy(false);

                Id(x => x.Id, x => x.Generator(Generators.Identity));

                Property(x => x.Name, x => x.Length(100));
                Property(x => x.Value, x => x.Length(1000));
            }
        }
    }
}