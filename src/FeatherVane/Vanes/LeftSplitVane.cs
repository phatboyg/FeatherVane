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
    using System;


    /// <summary>
    /// Strips the left payload and carries it in an out-of-band payload
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    public class LeftSplitVane<TLeft, TRight> :
        Vane<Tuple<TLeft, TRight>>
    {
        readonly Feather<TRight> _next;
        readonly Vane<Tuple<TLeft, TRight>> _outputVane;

        public LeftSplitVane(Feather<TRight> next, Vane<Tuple<TLeft, TRight>> outputVane)
        {
            _outputVane = outputVane;
            _next = next;
        }

        void Vane<Tuple<TLeft, TRight>>.Compose(Composer composer, Payload<Tuple<TLeft, TRight>> payload)
        {
            composer.Execute(() =>
                {
                    Payload<TRight> nextPayload = payload.SplitRight();

                    var output = new LeftMergeVane<TLeft, TRight>(payload.Data.Item1, _outputVane);

                    return TaskComposer.Compose(_next, nextPayload, output, composer.CancellationToken);
                });
        }
    }
}