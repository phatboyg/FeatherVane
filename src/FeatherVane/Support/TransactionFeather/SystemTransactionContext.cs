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
namespace FeatherVane.Support.TransactionFeather
{
    using System.Transactions;


    public class SystemTransactionContext :
        TransactionContext
    {
        readonly CommittableTransaction _transaction;
        bool _disposed;

        public SystemTransactionContext(TransactionOptions options)
        {
            _transaction = new CommittableTransaction(options);
        }

        public Transaction Transaction
        {
            get { return _transaction; }
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _transaction.Dispose();

            _disposed = true;
        }
    }
}