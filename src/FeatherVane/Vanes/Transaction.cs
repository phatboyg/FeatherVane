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
        FeatherVane<T>
        where T : class
    {
        TransactionScopeOption _scopeOptions;

        public Transaction()
        {
            _scopeOptions = TransactionScopeOption.Required;
        }

        public Handler<T> GetHandler(Payload<T> payload, Vane<T> next)
        {
            Handler<T> nextHandler = next.GetHandler(payload);

            return new TransactionHandler(_scopeOptions, nextHandler);
        }

        class TransactionHandler :
            Handler<T>
        {
            readonly Handler<T> _nextHandler;
            readonly TransactionScopeOption _options;

            public TransactionHandler(TransactionScopeOption options, Handler<T> nextHandler)
            {
                _options = options;
                _nextHandler = nextHandler;
            }

            public void Handle(Payload<T> payload)
            {
                using (var scope = new TransactionScope(_options))
                {
                    _nextHandler.Handle(payload);

                    scope.Complete();
                }
            }
        }
    }
}