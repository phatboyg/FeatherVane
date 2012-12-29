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
namespace FeatherVane.Tests.Benchmarks
{
    using System;
    using FeatherVane.Messaging;
    using FeatherVane.Messaging.Payloads;
    using FeatherVane.Messaging.Vanes;
    using FeatherVane.NHibernateIntegration.Vanes;
    using NHibernate;
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernateIntegration;
    using Vanes;


    public class NHibernateThroughput :
        Throughput
    {
        readonly ISessionFactory _sessionFactory;
        readonly Vane<Message> _vane;

        public NHibernateThroughput()
        {
            var sessionFactoryProvider = new SqlLiteSessionFactoryProvider(typeof(TargetConsumerMap));

            _sessionFactory = sessionFactoryProvider.GetSessionFactory();

            var consumerVane = new MessageConsumerVane<Subject, SubjectConsumer>(x => x.Consume);
            Vane<Tuple<Message<Subject>, SubjectConsumer>> finalVane = VaneFactory.Success(consumerVane);

            var id = new Id<Message<Subject>, int>(x => x.Body.Id);
            var factory = new Factory<SubjectConsumer>(() => new SubjectConsumer());
            var loadVane = new Load<SubjectConsumer, int>(_sessionFactory, id, factory);

            SourceVane<SubjectConsumer> sourceVane = VaneFactory.Source(loadVane);
            var spliceVane = new Splice<Message<Subject>, SubjectConsumer>(finalVane, sourceVane);
            Vane<Message<Subject>> vane = VaneFactory.Success(spliceVane);

            var messageVane = new MessageVane<Subject>(vane);

            var fanOutVane = new Fanout<Message>(new[] {messageVane});

            _vane = VaneFactory.Success(fanOutVane);

            var subject = new SubjectConsumer {Name = "Joe", Value = "Cool"};

            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                session.Save(subject);
                tx.Commit();
            }

            if (subject.Id != 1)
                throw new InvalidOperationException("First insert should be 1");
        }

        public void Execute(Subject subject)
        {
            var messagePayload = new MessagePayload<Subject>(subject);

            _vane.Execute(messagePayload);
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