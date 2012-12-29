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
namespace FeatherVane.NHibernateIntegration.Vanes
{
    using System;
    using NHibernate;


    /// <summary>
    /// Wraps a factory method into a SourceVane, allowing objects to be created within the
    /// scope of an execution with full lifecycle management.
    /// </summary>
    /// <typeparam name="T">The vane type of the main vane</typeparam>
    /// <typeparam name="TId">The identity type for the entity</typeparam>
    public class Load<T, TId> :
        SourceVane<T>,
        AcceptVaneVisitor
    {
        readonly SourceVane<TId> _id;
        readonly SourceVane<T> _missing;
        readonly ISessionFactory _sessionFactory;

        public Load(ISessionFactory sessionFactory, SourceVane<TId> id, SourceVane<T> missing)
        {
            _sessionFactory = sessionFactory;
            _id = id;
            _missing = missing;
        }

        void SourceVane<T>.Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, T>> next)
        {
            composer.Execute(() =>
                {
                    var get = new Get<TPayload>(_sessionFactory, _missing, next);

                    return TaskComposer.Compose(_id, payload, get, composer.CancellationToken);
                });
        }


        class Get<TPayload> :
            Vane<Tuple<TPayload, TId>>
        {
            readonly SourceVane<T> _missing;
            readonly Vane<Tuple<TPayload, T>> _next;
            readonly ISessionFactory _sessionFactory;

            internal Get(ISessionFactory sessionFactory, SourceVane<T> missing, Vane<Tuple<TPayload, T>> next)
            {
                _sessionFactory = sessionFactory;
                _missing = missing;
                _next = next;
            }

            void Vane<Tuple<TPayload, TId>>.Compose(Composer composer, Payload<Tuple<TPayload, TId>> payload)
            {
                ISession session = null;
                ITransaction transaction = null;

                composer.Execute(() =>
                    {
                        session = _sessionFactory.OpenSession();
                        transaction = session.BeginTransaction();
                        var data = session.Get<T>(payload.Data.Item2, LockMode.Upgrade);
                        if (data == null)
                        {
                            return TaskComposer.Compose(_missing, payload.CreateProxy(payload.Data.Item1), _next,
                                composer.CancellationToken);
                        }

                        Payload<Tuple<TPayload, T>> nextPayload =
                            payload.CreateProxy(Tuple.Create(payload.Data.Item1, data));

                        return TaskComposer.Compose(_next, nextPayload, composer.CancellationToken);
                    });

                composer.Execute(() =>
                    {
                        if (transaction.IsActive)
                            transaction.Commit();
                    });

                composer.Finally(() =>
                    {
                        if (transaction != null)
                            transaction.Dispose();

                        if (session != null)
                            session.Dispose();
                    });
            }
        }


        public bool Accept(VaneVisitor visitor)
        {
            return visitor.Visit(_id) && visitor.Visit(_missing);
        }
    }
}