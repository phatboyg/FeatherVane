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
namespace FeatherVane
{
    using System;
    using System.Transactions;


    public interface TransactionContext :
        IDisposable
    {
        /// <summary>
        /// Returns the current transaction scope, creating a dependent scope if a thread switch
        /// occurred
        /// </summary>
        Transaction Current { get; }

        /// <summary>
        /// Complete the transaction scope
        /// </summary>
        void Complete();

        /// <summary>
        /// Creates a transaction scope using the existing transaction
        /// </summary>
        /// <returns></returns>
        TransactionScope CreateTransactionScope();
    }
}