// Copyright 2012-2013 Chris Patterson
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
    using System.Collections.Generic;
    using System.IO;
    using FeatherVane.NHibernateIntegration;
    using FeatherVane.NHibernateIntegration.SourceVanes;
    using NHibernate;
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;
    using NUnit.Framework;
    using SourceVanes;
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
            Vane<A> vane =
                VaneFactory.New<A>(vc =>
                    {
                        vc.Profiler(v => v.Threshold(TimeSpan.FromMilliseconds(1)).SetOutput(Console.Out));

                        vc.Load(xl => xl.Object<Subject, int>(load =>
                            {
                                load.UseSessionFactory(SessionFactory);
                                load.Id(msg => msg.Id);
                                load.Missing(mx =>
                                    {
                                        mx.Factory(() => new Subject());
                                        mx.Log(lx => lx.SetOutput(Console.Out)
                                                       .SetFormat(fs => "Created subject: " + fs.Data.Id));
                                    });

                                load.Log(v => v.SetOutput(Console.Out)
                                               .SetFormat(fs => "Loaded subject: " + fs.Data.Id));
                            }, v => v.Execute(payload =>
                                {
                                    payload.Data.Item2.Id = payload.Data.Item1.Id;
                                    payload.Data.Item2.Name = payload.Data.Item1.GetType().Name;
                                    payload.Data.Item2.Value = payload.Data.Item1.Value;
                                })));
                    });


            vane.RenderGraphToFile(new FileInfo("NHibernateTestGraph.png"));

            var a = new A {Id = _id, Value = "Joe"};
            vane.Execute(a);
            vane.Execute(a);
            var a2 = new A {Id = 47, Value = "Cool"};
            vane.Execute(a2);

            using (ISession session = SessionFactory.OpenSession())
            {
                IList<Subject> query = session.QueryOver<Subject>()
                                              .List();

                foreach (Subject row in query)
                    Console.WriteLine("{0}: {1} = {2}", row.Id, row.Name, row.Value);
            }
        }

        [Test]
        public void Should_throw_if_an_object_is_not_found()
        {
            var executeVane = new ExecuteVane<Tuple<A, Subject>>(x => { });
            Vane<Tuple<A, Subject>> finalVane = VaneFactory.Success(executeVane);

            var id = new IdentitySourceVane<A, int>(x => x.Id);
            var factory = new MissingSourceVane<Subject>();
            var loadVane = new LoadSourceVane<Subject, int>(SessionFactory, id, factory);

            SourceVane<Subject> sourceVane = VaneFactory.Source(loadVane);
            var spliceVane = new SpliceVane<A, Subject>(finalVane, sourceVane);

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

            public string Value { get; set; }
        }


        public class Subject
        {
            public int Id { get; set; }
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