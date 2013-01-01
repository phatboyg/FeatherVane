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
namespace FeatherVane.SourceVaneBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SourceVanes;
    using VaneBuilders;


    public class SourceVaneBuilder<T> :
        VaneBuilder<T>
    {
        readonly Func<SourceVane<T>> _tailFactory;
        readonly IList<FeatherVane<T>> _featherVanes;

        public SourceVaneBuilder(Func<SourceVane<T>> tailFactory)
        {
            _tailFactory = tailFactory;
            _featherVanes = new List<FeatherVane<T>>();
        }

        public void Add(FeatherVane<T> featherVaneFactory)
        {
            _featherVanes.Add(featherVaneFactory);
        }

        public SourceVane<T> Build()
        {
            return Build(_tailFactory(), _featherVanes);
        }

        static SourceVane<T> Build(SourceVane<T> head, IEnumerable<FeatherVane<T>> vanes)
        {
            return vanes.Aggregate(head, (x, vane) => new NextSource<T>(x, vane));
        }
    }
}