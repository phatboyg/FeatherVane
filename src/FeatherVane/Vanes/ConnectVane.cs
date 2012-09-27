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
    using Execution;

    class ConnectVane<T> :
        Vane<T>,
        AcceptVaneVisitor
    {
        readonly Vane<T> _next;
        readonly FeatherVane<T> _vane;

        public ConnectVane(FeatherVane<T> vane, Vane<T> next)
        {
            _vane = vane;
            _next = next;
        }

        public bool Accept(VaneVisitor visitor)
        {
            return visitor.Visit(_vane, x => visitor.Visit(_next));
        }

        public Agenda<T> Plan(Planner<T> planner, Payload<T> payload)
        {
            return _vane.Plan(planner, payload, _next);
        }
    }
}