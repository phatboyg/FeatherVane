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
    using Taskell;


    /// <summary>
    /// Carries an out-of-band payload alongside an adjacent vane of a different type
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    public class LeftMergeVane<TLeft, TRight> :
        Vane<TRight>
    {
        readonly TLeft _data;
        readonly Vane<Tuple<TLeft, TRight>> _outputVane;

        public LeftMergeVane(TLeft data, Vane<Tuple<TLeft, TRight>> outputVane)
        {
            _data = data;
            _outputVane = outputVane;
        }

        public void Compose(Composer composer, Payload<TRight> payload)
        {
            composer.Execute(() => composer.ComposeTask(_outputVane, payload.MergeLeft(_data)));
        }
    }
}