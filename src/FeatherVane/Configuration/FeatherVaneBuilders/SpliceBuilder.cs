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
namespace FeatherVane.FeatherVaneBuilders
{
    using System;
    using System.Collections.Generic;
    using Configurators;
    using Vanes;


    public class SpliceBuilder<T, TSource> :
        FeatherVaneBuilder<T>
    {
        readonly VaneFactory<Tuple<T, TSource>> _outputVaneFactory;
        readonly SourceVaneFactory<TSource> _sourceVaneFactory;
        readonly IList<FeatherVaneBuilder<Tuple<T, TSource>>> _vanes;

        public SpliceBuilder(VaneFactory<Tuple<T, TSource>> outputVaneFactory,
            SourceVaneFactory<TSource> sourceVaneFactory)
        {
            _outputVaneFactory = outputVaneFactory;
            _sourceVaneFactory = sourceVaneFactory;

            _vanes = new List<FeatherVaneBuilder<Tuple<T, TSource>>>();
        }

        FeatherVane<T> FeatherVaneBuilder<T>.Build()
        {
            Vane<Tuple<T, TSource>> outputVane = _outputVaneFactory.Create();
            SourceVane<TSource> sourceVane = _sourceVaneFactory.Create();

            var splice = new Splice<T, TSource>(outputVane, sourceVane);

            return splice;
        }
    }
}