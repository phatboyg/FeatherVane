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
namespace FeatherVane.Vanes
{
    using System.Transactions;


    /// <summary>
    /// Adds a TransactionScope to the Vane, allowing transactional operations
    /// </summary>
    /// <typeparam name="T">The Vane type</typeparam>
    public class Transaction<T> :
        FeatherVane<T>
    {
        readonly TransactionScopeOption _scopeOptions;

        public Transaction(TransactionScopeOption scopeOptions)
        {
            _scopeOptions = scopeOptions;
        }

        public Transaction()
        {
            _scopeOptions = TransactionScopeOption.Required;
        }

        void FeatherVane<T>.Build(Builder<T> builder, Payload<T> payload, Vane<T> next)
        {
            TransactionScopeOption options = _scopeOptions;

            TransactionScope createdScope = null;
            builder.Execute(() => payload.GetOrAdd(() =>
                {
                    createdScope = new TransactionScope(options);
                    return createdScope;
                }));

            next.Build(builder, payload);

            builder.Execute(() =>
                {
                    if (createdScope != null)
                        createdScope.Complete();
                });

            builder.Finally(() =>
                {
                    if (createdScope != null)
                        createdScope.Dispose();
                });
        }
    }
}