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
namespace FeatherVane.Tests.Benchmarks
{
    using System;
    using FeatherVane.Messaging;
    using FeatherVane.Messaging.Payloads;
    using FeatherVane.NHibernateIntegration;
    using NHibernate;
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernateIntegration;


    public class NHibernateThroughput :
        Throughput
    {
        readonly ISessionFactory _sessionFactory;
        readonly Vane<Message> _vane;

        public NHibernateThroughput()
        {
            var sessionFactoryProvider = new SqlLiteSessionFactoryProvider(typeof(TargetConsumerMap));

            _sessionFactory = sessionFactoryProvider.GetSessionFactory();

            _vane = VaneFactory.New<Message>(m =>
                {
                    m.MessageType<Subject>(sub =>
                        {
                            sub.Load(y => y.Object<SubjectConsumer, int>(load =>
                                {
                                    load.UseSessionFactory(_sessionFactory);
                                    load.Id(d => d.Body.Id);
                                    load.Missing(mv => mv.Factory(() => new SubjectConsumer()));
                                }, subj => subj.Consume(t => t.Consume)));
                        });
                });

            PreloadData(_sessionFactory);
        }

        public void Execute(Subject subject)
        {
            var messagePayload = new MessagePayload<Subject>(subject);

            _vane.Execute(messagePayload);
        }

        void PreloadData(ISessionFactory sessionFactory)
        {
            var subject = new SubjectConsumer {Name = "Joe", Value = "Cool"};

            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                session.Save(subject);
                tx.Commit();
            }

            if (subject.Id != 1)
                throw new InvalidOperationException("First insert should be 1");
        }


        public class SubjectConsumer
        {
            public int Id { get; private set; }
            public string Name { get; set; }
            public string Value { get; set; }

            public void Consume(Payload payload, Message<Subject> message)
            {
            }
        }


        public class TargetConsumerMap :
            ClassMapping<SubjectConsumer>
        {
            public TargetConsumerMap()
            {
                Table("SubjectConsumer");
                Lazy(false);

                Id(x => x.Id, x => x.Generator(Generators.Identity));

                Property(x => x.Name, x => x.Length(100));
                Property(x => x.Value, x => x.Length(1000));
            }
        }
    }
}