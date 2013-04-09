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
namespace FeatherVane.NHibernateIntegration.SourceVanes
{
    using System;
    using NHibernate;
    using Taskell;


    /// <summary>
    /// Wraps a factory method into a SourceVane, allowing objects to be created within the
    /// scope of an execution with full lifecycle management.
    /// </summary>
    /// <typeparam name="T">The vane type of the main vane</typeparam>
    /// <typeparam name="TId">The identity type for the entity</typeparam>
    public class LoadSourceVane<T, TId> :
        SourceVane<T>,
        AcceptVaneVisitor
    {
        readonly SourceVane<TId> _identityVane;
        readonly SourceVane<T> _missingVane;
        readonly ISessionFactory _sessionFactory;

        public LoadSourceVane(ISessionFactory sessionFactory, SourceVane<TId> identityVane, SourceVane<T> missingVane)
        {
            _sessionFactory = sessionFactory;
            _identityVane = identityVane;
            _missingVane = missingVane;
        }

        public bool Accept(VaneVisitor visitor)
        {
            return visitor.Visit(_identityVane) && visitor.Visit(_missingVane);
        }

        void SourceVane<T>.Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, T>> next)
        {
            composer.Execute(() =>
                {
                    var get = new Get<TPayload>(_sessionFactory, _missingVane, next);

                    return composer.ComposeTask(_identityVane, payload, get);
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
                        
                        var leftPayload = payload.SplitLeft();

                        var data = session.Get<T>(payload.Data.Item2, LockMode.Upgrade);
                        if (data == null)
                        {
                            var save = new Save<TPayload>(session, _next);
                            return composer.ComposeTask(_missing, leftPayload, save);
                        }

                        return composer.ComposeTask(_next, leftPayload.MergeRight(data));
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


        class Save<TPayload> :
            Vane<Tuple<TPayload, T>>
        {
            readonly Vane<Tuple<TPayload, T>> _next;
            readonly ISession _session;

            public Save(ISession session, Vane<Tuple<TPayload, T>> next)
            {
                _session = session;
                _next = next;
            }

            public void Compose(Composer composer, Payload<Tuple<TPayload, T>> payload)
            {
                _next.Compose(composer, payload);

                composer.Execute(() => _session.Save(payload.Data.Item2));
            }
        }
    }
}