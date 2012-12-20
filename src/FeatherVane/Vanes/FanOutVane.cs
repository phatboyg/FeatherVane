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
    using System.Collections.Generic;
    using System.Linq;


    /// <summary>
    /// A fan-out Vane composes over a list of subsequent vanes for every execution
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FanOutVane<T> :
        FeatherVane<T>,
        AcceptVaneVisitor

    {
        readonly IList<FeatherVane<T>> _vanes;

        public FanOutVane(IEnumerable<FeatherVane<T>> vanes)
        {
            _vanes = vanes.ToList();
        }

        public bool Accept(VaneVisitor visitor)
        {
            return visitor.Visit(this, x => _vanes.All(visitor.Visit));
        }

        public void Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            for (int i = 0; i < _vanes.Count; i++)
                _vanes[i].Compose(composer, payload, next);
        }
    }
}