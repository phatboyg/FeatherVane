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
namespace FeatherVane.NHibernateIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Cfg.Loquacious;
    using NHibernate.Cfg.MappingSchema;
    using NHibernate.Mapping.ByCode;
    using NHibernate.Tool.hbm2ddl;


    /// <summary>
    /// Makes it easy to create an NHibernate Session factory using the mappings
    /// </summary>
    public class NHibernateSessionFactoryProvider
    {
        static readonly Mutex _sessionFactoryMutex = new Mutex();
        readonly Configuration _configuration;
        readonly Action<IDbIntegrationConfigurationProperties> _databaseIntegration;
        readonly IEnumerable<Type> _mappedTypes;
        bool _computed;
        ISessionFactory _sessionFactory;

        public NHibernateSessionFactoryProvider(IEnumerable<Type> mappedTypes)
        {
            _mappedTypes = mappedTypes;

            _configuration = CreateConfiguration();
        }

        public NHibernateSessionFactoryProvider(IEnumerable<Type> mappedTypes,
            Action<IDbIntegrationConfigurationProperties> databaseIntegration)
        {
            _mappedTypes = mappedTypes;
            _databaseIntegration = databaseIntegration;
            _configuration = CreateConfiguration();
        }

        public Configuration Configuration
        {
            get { return _configuration; }
        }

        /// <summary>
        /// Builds the session factory and returns the ISessionFactory. If it was already
        /// built, the same instance is returned.
        /// </summary>
        /// <returns></returns>
        public virtual ISessionFactory GetSessionFactory()
        {
            if (_computed)
                return _sessionFactory;

            return CreateSessionFactory();
        }

        /// <summary>
        /// Update the schema in the database
        /// </summary>
        public void UpdateSchema()
        {
            new SchemaUpdate(_configuration).Execute(false, true);
        }

        ModelMapper CreateModelMapper()
        {
            var mapper = new ModelMapper();

            mapper.AfterMapClass += (inspector, type, customizer) => { };

            mapper.AfterMapProperty += (inspector, member, customizer) =>
                {
                    Type memberType = member.LocalMember.GetPropertyOrFieldType();

                    if (memberType.IsGenericType
                        && typeof(Nullable<>).IsAssignableFrom(memberType.GetGenericTypeDefinition()))
                        customizer.NotNullable(false);
                    else if (!typeof(string).IsAssignableFrom(memberType))
                        customizer.NotNullable(true);
                };

            mapper.AddMappings(_mappedTypes);

            return mapper;
        }


        ISessionFactory CreateSessionFactory()
        {
            try
            {
                bool acquired = _sessionFactoryMutex.WaitOne();
                if (!acquired)
                    throw new NHibernateIntegrationException("Waiting for access to create session factory failed.");

                ISessionFactory sessionFactory = _configuration.BuildSessionFactory();

                _sessionFactory = sessionFactory;
                _computed = true;

                return sessionFactory;
            }
            catch (Exception ex)
            {
                throw new NHibernateIntegrationException("Exception creating NHibernate session factory", ex);
            }
            finally
            {
                _sessionFactoryMutex.ReleaseMutex();
            }
        }

        Configuration ApplyDatabaseIntegration(Configuration configuration)
        {
            if (_databaseIntegration == null)
                configuration = configuration.Configure();

            configuration.DataBaseIntegration(c =>
                {
                    c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                    c.SchemaAction = SchemaAutoAction.Validate;

                    if (_databaseIntegration != null)
                        _databaseIntegration(c);
                });

            return configuration;
        }

        Configuration CreateConfiguration()
        {
            ModelMapper mapper = CreateModelMapper();

            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var configuration = new Configuration();

            configuration = ApplyDatabaseIntegration(configuration);

            configuration.AddMapping(domainMapping);

            return configuration;
        }
    }
}