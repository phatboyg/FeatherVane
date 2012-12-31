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
namespace FeatherVane.SourceVaneBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FeatherVaneBuilders;
    using VaneBuilders;
    using Vanes;


    public class SourceVaneBuilder<T> :
        VaneBuilder<T>
    {
        readonly IList<FeatherVaneBuilder<T>> _featherVaneBuilders;
        readonly Func<SourceVane<T>> _tailFactory;

        public SourceVaneBuilder(Func<SourceVane<T>> tailFactory)
        {
            _tailFactory = tailFactory;
            _featherVaneBuilders = new List<FeatherVaneBuilder<T>>();
        }

        public void Add(FeatherVaneBuilder<T> featherVaneBuilder)
        {
            _featherVaneBuilders.Add(featherVaneBuilder);
        }

        public SourceVane<T> Build()
        {
            return Build(_tailFactory(), _featherVaneBuilders.Select(builder => builder.Build()));
        }

        static SourceVane<T> Build(SourceVane<T> head, IEnumerable<FeatherVane<T>> vanes)
        {
            return vanes.Aggregate(head, (x, vane) => new NextSource<T>(x, vane));
        }
    }
}