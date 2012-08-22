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

    public class Transaction<T> :
        Vane<T>
        where T : class
    {
        TransactionScopeOption _scopeOptions;

        public Transaction()
        {
            _scopeOptions = TransactionScopeOption.Required;
        }

        public VaneHandler<T> GetHandler(VaneContext<T> context, NextVane<T> next)
        {
            VaneHandler<T> nextHandler = next.GetHandler(context);

            return new TransactionVaneHandler(_scopeOptions, nextHandler);
        }

        class TransactionVaneHandler :
            VaneHandler<T>
        {
            readonly VaneHandler<T> _nextHandler;
            readonly TransactionScopeOption _options;

            public TransactionVaneHandler(TransactionScopeOption options, VaneHandler<T> nextHandler)
            {
                _options = options;
                _nextHandler = nextHandler;
            }

            public void Handle(VaneContext<T> context)
            {
                using (var scope = new TransactionScope(_options))
                {
                    _nextHandler.Handle(context);

                    scope.Complete();
                }
            }
        }
    }
}