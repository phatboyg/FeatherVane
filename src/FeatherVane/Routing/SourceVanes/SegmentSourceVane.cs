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
namespace FeatherVane.Routing.SourceVanes
{
    using System;
    using Taskell;


    /// <summary>
    /// Selects a segment from a RoutingContext and if present, invokes the next vane
    /// </summary>
    public class SegmentSourceVane :
        SourceVane<RoutingContext, string>
    {
        readonly int _position;

        public SegmentSourceVane(int position)
        {
            _position = position;
        }

        /// <summary>
        /// The segment position in the URI path
        /// </summary>
        public int Position
        {
            get { return _position; }
        }

        public void Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, string>> next)
            where TPayload : RoutingContext
        {
            composer.Execute(() =>
                {
                    string segmentValue = payload.Data.Segment(_position);
                    if (segmentValue == null)
                        return TaskUtil.Completed();

                    return CompositionExtensions.Compose(next, payload.MergeRight(segmentValue), composer.CancellationToken);
                });
        }
    }
}