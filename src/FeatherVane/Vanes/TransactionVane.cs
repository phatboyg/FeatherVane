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
namespace FeatherVane.Vanes
{
    using System;
    using System.Transactions;


    /// <summary>
    /// Adds a TransactionScope to the Vane, allowing transactional operations
    /// </summary>
    /// <typeparam name="T">The Vane type</typeparam>
    public class TransactionVane<T> :
        FeatherVane<T>
    {
        readonly TransactionOptions _options;

        public TransactionVane(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
                timeout = TimeSpan.FromSeconds(30);

            _options = new TransactionOptions();
            _options.IsolationLevel = isolationLevel;
            _options.Timeout = timeout;
        }

        void FeatherVane<T>.Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            TransactionContext transactionContext = null;
            composer.Execute(() => payload.GetOrAdd(() =>
                {
                    transactionContext = new SystemTransactionContext(_options);
                    return transactionContext;
                }));

            next.Compose(composer, payload);

            composer.Execute(() =>
                {
                    if (transactionContext != null)
                        transactionContext.Commit();
                });

            composer.Finally(() =>
                {
                    if (transactionContext != null)
                        transactionContext.Dispose();
                });
        }
    }
}