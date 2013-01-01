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
namespace FeatherVane.VaneBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Vanes;


    public class VaneBuilderImpl<T> :
        VaneBuilder<T>
    {
        readonly IList<FeatherVane<T>> _featherVanes;
        readonly Func<Vane<T>> _tailFactory;

        public VaneBuilderImpl(Func<Vane<T>> tailFactory)
        {
            _tailFactory = tailFactory;
            _featherVanes = new List<FeatherVane<T>>();
        }

        void VaneBuilder<T>.Add(FeatherVane<T> featherVane)
        {
            _featherVanes.Add(featherVane);
        }

        public Vane<T> Build()
        {
            Vane<T> tail = _tailFactory();

            return _featherVanes
                .Reverse()
                .Aggregate(tail, (x, vane) => new NextVane<T>(vane, x));
        }
    }
}