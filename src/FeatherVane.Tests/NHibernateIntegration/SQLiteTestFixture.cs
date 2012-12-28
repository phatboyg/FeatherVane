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
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate;
    using NUnit.Framework;


    [TestFixture]
    public abstract class SQLiteTestFixture
    {
        IEnumerable<Type> _mapTypes;

        public SQLiteTestFixture()
        {
            _mapTypes = Enumerable.Empty<Type>();
        }

        protected void Map(params Type[] types)
        {
            _mapTypes = _mapTypes.Concat(types).Distinct();
        }

        [TestFixtureSetUp]
        public void SQLiteTestFixtureSetup()
        {
            var sessionFactoryProvider = new SqlLiteSessionFactoryProvider(_mapTypes.ToArray());

            SessionFactory = sessionFactoryProvider.GetSessionFactory();
        }

        [TestFixtureTearDown]
        public void SQLiteTestFixtureTearDown()
        {
            SessionFactory.Dispose();
        }

        protected ISessionFactory SessionFactory { get; private set; }
    }
}