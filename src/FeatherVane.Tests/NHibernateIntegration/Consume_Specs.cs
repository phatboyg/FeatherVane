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
    using Benchmarks;
    using FeatherVane.Messaging;
    using FeatherVane.NHibernateIntegration;
    using NHibernate;
    using NUnit.Framework;


    [TestFixture]
    public class Consume_method
    {
        [Test]
        public void Should_be_composable_before_load()
        {
            var sessionFactoryProvider =
                new SqlLiteSessionFactoryProvider(typeof(NHibernateThroughput.TargetConsumerMap));

            using (ISessionFactory sessionFactory = sessionFactoryProvider.GetSessionFactory())
            {
                Vane<Message> vane = VaneFactory.New<Message>(m =>
                    {
                        m.MessageType<Subject>(sub =>
                            {
                                sub.Load(y => y.Object<NHibernateThroughput.SubjectConsumer, int>(load =>
                                    {
                                        load.UseSessionFactory(sessionFactory);
                                        load.Id(d => d.Body.Id);
                                        load.Missing(mv => mv.Factory(() => new NHibernateThroughput.SubjectConsumer()));
                                    }, subj => subj.Consume(t => t.Consume)));
                            });
                    });
            }
        }
    }
}