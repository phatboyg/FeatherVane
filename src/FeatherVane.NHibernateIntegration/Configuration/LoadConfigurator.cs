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
namespace FeatherVane.NHibernateIntegration
{
    using System;


    public interface LoadConfigurator<T> :
        FeatherVaneConfigurator<T>
    {
        /// <summary>
        /// Loads an object from NHibernate of the type specified, using the configuration
        /// information provided.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TIdentity"></typeparam>
        /// <param name="configureObject"></param>
        /// <param name="configureVane"></param>
        void Object<TSource, TIdentity>(Action<LoadObjectVaneConfigurator<T, TSource, TIdentity>> configureObject,
            Action<LoadVaneConfigurator<T, TSource>> configureVane);
    }
}