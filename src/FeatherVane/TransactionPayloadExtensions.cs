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
    using System.Transactions;


    public static class TransactionPayloadExtensions
    {
        /// <summary>
        /// Create a transaction scope using the TransactionVane, to ensure that any transactions 
        /// are carried between any threads.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static TransactionScope CreateTransactionScope<T>(this Payload<T> payload)
        {
            var context = payload.Get<TransactionContext>();

            return context.CreateTransactionScope();
        }
    }
}