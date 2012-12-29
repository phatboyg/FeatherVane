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
    using Internals.Caching;


    /// <summary>
    /// A fan-out Vane composes over a list of subsequent vanes for every execution
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Fanout<T> :
        FeatherVane<T>,
        AcceptVaneVisitor
    {
        readonly Cache<FeatherVane<T>, FeatherVane<T>> _vanes;

        public Fanout(IEnumerable<FeatherVane<T>> vanes)
        {
            _vanes = new ConcurrentCache<FeatherVane<T>, FeatherVane<T>>
                {
                    KeySelector = x => x
                };
            _vanes.Fill(vanes);
        }

        public bool Accept(VaneVisitor visitor)
        {
            return _vanes.All(visitor.Visit);
        }

        public void Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            foreach (var vane in _vanes)
                vane.Compose(composer, payload, next);
        }

        public void Add(FeatherVane<T> vane)
        {
            _vanes.AddValue(vane);
        }

        public void Remove(FeatherVane<T> vane)
        {
            _vanes.RemoveValue(vane);
        }
    }
}