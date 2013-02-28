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
namespace FeatherVane.FeatherVaneConfigurators
{
    using System;
    using System.Collections.Generic;
    using System.Transactions;
    using Configurators;
    using Feathers;
    using VaneBuilders;


    public class TransactionConfiguratorImpl<T> :
        TransactionConfigurator<T>,
        VaneBuilderConfigurator<T>
    {
        IsolationLevel _isolationLevel;
        TimeSpan _timeout;

        public TransactionConfiguratorImpl()
        {
            _isolationLevel = IsolationLevel.ReadCommitted;
            _timeout = TimeSpan.FromSeconds(30);
        }

        public TransactionConfigurator<T> SetTimeout(TimeSpan timeout)
        {
            _timeout = timeout;
            return this;
        }

        public TransactionConfigurator<T> SetIsolationLevel(IsolationLevel isolationLevel)
        {
            _isolationLevel = isolationLevel;
            return this;
        }

        void VaneBuilderConfigurator<T>.Configure(VaneBuilder<T> builder)
        {
            var transaction = new TransactionFeather<T>(_isolationLevel, _timeout);
            builder.Add(transaction);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            if (_timeout == TimeSpan.Zero)
                yield return this.Failure("Timeout", "Must not be zero");
        }
    }
}