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
        FeatherVane<T>,
        AgendaItem<T>
    {
        readonly TransactionScopeOption _scopeOptions;

        public Transaction()
        {
            _scopeOptions = TransactionScopeOption.Required;
        }

        public bool Execute(Agenda<T> agenda)
        {
            TransactionScope scope = agenda.Payload.GetOrAdd(() => new TransactionScope(_scopeOptions));

            if (agenda.Execute())
            {
                scope.Complete();
                scope.Dispose();

                return true;
            }

            return false;
        }

        public bool Compensate(Agenda<T> agenda)
        {
            var transactionScope = agenda.Payload.Get<TransactionScope>();
            transactionScope.Dispose();

            return agenda.Compensate();
        }

        public Agenda<T> Plan(Planner<T> planner, Payload<T> payload, Vane<T> next)
        {
            planner.Add(this);

            return next.Plan(planner, payload);
        }
    }
}