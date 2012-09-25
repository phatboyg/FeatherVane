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
        Step<T>
        where T : class
    {
        readonly TransactionScopeOption _scopeOptions;

        public Transaction()
        {
            _scopeOptions = TransactionScopeOption.Required;
        }

        public Plan<T> AssignPlan(Planner<T> planner, Payload<T> payload, Vane<T> next)
        {
            planner.Add(this);

            return next.AssignPlan(planner, payload);
        }

        public bool Execute(Plan<T> plan)
        {
            bool ok;
            using (var scope = new TransactionScope(_scopeOptions))
            {
                ok = plan.Execute();

                if (ok)
                    scope.Complete();
            }

            return ok;
        }

        public bool Compensate(Plan<T> plan)
        {
            return plan.Compensate();
        }
    }
}