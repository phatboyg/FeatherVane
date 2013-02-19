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
namespace FeatherVane.Vanes
{
    using System;


    /// <summary>
    /// Splices a vane with a source vane
    /// </summary>
    /// <typeparam name="T">The primary vane type</typeparam>
    /// <typeparam name="TSource">The source vane type</typeparam>
    public class SpliceSourceVane<T, TSource> :
        FeatherVane<T>,
        AcceptVaneVisitor
    {
        readonly Vane<Tuple<T, TSource>> _output;
        readonly SourceVane<T, TSource> _sourceVane;

        public SpliceSourceVane(Vane<Tuple<T, TSource>> output, SourceVane<T, TSource> sourceVane)
        {
            _output = output;
            _sourceVane = sourceVane;
        }

        public bool Accept(VaneVisitor visitor)
        {
            return visitor.Visit(_sourceVane) && visitor.Visit(_output);
        }

        void FeatherVane<T>.Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            composer.Execute(() => TaskComposer.Compose(_sourceVane, payload, _output, composer.CancellationToken));

            next.Compose(composer, payload);
        }
    }
}