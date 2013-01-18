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
    using System.Threading;
    using System.Transactions;
    using Internals.Caching;


    public class SystemTransactionContext :
        TransactionContext
    {
        readonly Cache<int, DependentTransaction> _dependentTransactions;
        readonly int _initialThreadId;
        readonly CommittableTransaction _transaction;
        bool _disposed;

        public SystemTransactionContext(TransactionOptions options)
        {
            _transaction = new CommittableTransaction(options);

            _initialThreadId = Thread.CurrentThread.ManagedThreadId;
            _dependentTransactions = new ConcurrentCache<int, DependentTransaction>();
        }

        public Transaction Current
        {
            get { return GetCurrent(); }
        }

        public void Complete()
        {
            CompleteTransaction();
        }

        public TransactionScope CreateTransactionScope()
        {
            return new TransactionScope(GetCurrent());
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            foreach (DependentTransaction transaction in _dependentTransactions)
                transaction.Dispose();

            _transaction.Dispose();
        }

        public void CompleteTransaction()
        {
            foreach (DependentTransaction transaction in _dependentTransactions)
            {
                transaction.Complete();
            }

            _transaction.Commit();
        }

        Transaction GetCurrent()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (threadId == _initialThreadId)
                return _transaction;

            return _dependentTransactions.Get(threadId, 
                id => _transaction.DependentClone(DependentCloneOption.BlockCommitUntilComplete));
        }
    }
}