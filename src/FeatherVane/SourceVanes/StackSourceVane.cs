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
    using Support.StackFeather;
    using Taskell;


    public class StackSourceVane<T> :
        SourceVane<T>,
        Stack<SourceVane<T>>
    {
        readonly System.Collections.Generic.Stack<SourceVane<T>> _vanes;

        public StackSourceVane(params SourceVane<T>[] vanes)
        {
            _vanes = new System.Collections.Generic.Stack<SourceVane<T>>(vanes);
        }

        public void Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, T>> next)
        {
            SourceVane<T> vane;
            lock (_vanes)
                vane = _vanes.Peek();

            vane.Compose(composer, payload, next);
        }

        void Stack<SourceVane<T>>.Push(SourceVane<T> vane)
        {
            lock (_vanes)
                _vanes.Push(vane);
        }

        void Stack<SourceVane<T>>.Pop(SourceVane<T> vane)
        {
            lock (_vanes)
            {
                if (_vanes.Peek() == vane)
                    _vanes.Pop();
            }
        }

        void Stack<SourceVane<T>>.Set(params SourceVane<T>[] vanes)
        {
            lock (_vanes)
            {
                _vanes.Clear();
                for (int i = 0; i < vanes.Length; i++)
                    _vanes.Push(vanes[i]);
            }
        }
    }
}