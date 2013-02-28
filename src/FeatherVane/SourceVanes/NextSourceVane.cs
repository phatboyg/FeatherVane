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
namespace FeatherVane.SourceVanes
{
    using System;
    using Vanes;


    /// <summary>
    /// Curries a SourceVane into a Vane, using the next Vane supplied during
    /// Build operations.
    /// </summary>
    /// <typeparam name="TSource">The source vane type</typeparam>
    public class NextSourceVane<TSource> :
        SourceVane<TSource>,
        AcceptVaneVisitor
    {
        readonly Feather<TSource> _next;
        readonly SourceVane<TSource> _sourceVane;

        /// <summary>
        /// Constructs a NextSource
        /// </summary>
        /// <param name="sourceVane">The FeatherVane to combine with the next Vane</param>
        /// <param name="nextVane"></param>
        public NextSourceVane(SourceVane<TSource> sourceVane, Feather<TSource> next)
        {
            _sourceVane = sourceVane;
            _next = next;
        }

        public SourceVane<TSource> Source
        {
            get { return _sourceVane; }
        }

        public Feather<TSource> Next
        {
            get { return _next; }
        }

        bool AcceptVaneVisitor.Accept(VaneVisitor visitor)
        {
            return visitor.Visit(_sourceVane) && visitor.Visit(_next);
        }

        void SourceVane<TSource>.Compose<TPayload>(Composer composer, Payload<TPayload> payload,
            Vane<Tuple<TPayload, TSource>> next)
        {
            var nextVane = new LeftSplitVane<TPayload, TSource>(_next, next);

            _sourceVane.Compose(composer, payload, nextVane);
        }
    }
}