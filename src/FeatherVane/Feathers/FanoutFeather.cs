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
namespace FeatherVane.Feathers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Internals.Caching;
    using Taskell;


    /// <summary>
    /// A fan-out Vane composes over a list of subsequent vanes for every execution
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Count: {Count}")]
    public class FanoutFeather<T> :
        Feather<T>,
        AcceptVaneVisitor
    {
        readonly Cache<Feather<T>, Feather<T>> _vanes;

        public FanoutFeather(IEnumerable<Feather<T>> vanes)
        {
            _vanes = new ConcurrentCache<Feather<T>, Feather<T>>
                {
                    KeySelector = x => x
                };
            _vanes.Fill(vanes);
        }

        public IEnumerable<Feather<T>> Vanes
        {
            get { return _vanes; }
        }

        public int Count
        {
            get { return _vanes.Count; }
        }

        bool AcceptVaneVisitor.Accept(VaneVisitor visitor)
        {
            return _vanes.All(visitor.Visit);
        }

        void Feather<T>.Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            foreach (var vane in _vanes)
                vane.Compose(composer, payload, next);
        }

        public void Add(Feather<T> vane)
        {
            _vanes.AddValue(vane);
        }

        public void Remove(Feather<T> vane)
        {
            _vanes.RemoveValue(vane);
        }
    }
}