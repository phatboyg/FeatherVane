﻿// Copyright 2012-2013 Chris Patterson
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
namespace FeatherVane.Feathers
{
    using Support.StackFeather;
    using Taskell;


    public class StackFeather<T> :
        Feather<T>,
        Stack<Vane<T>>
    {
        readonly System.Collections.Generic.Stack<Vane<T>> _vanes;

        public StackFeather(params Vane<T>[] vanes)
        {
            _vanes = new System.Collections.Generic.Stack<Vane<T>>(vanes);
        }

        public void Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            Vane<T> vane;
            lock (_vanes)
                vane = _vanes.Peek();

            vane.Compose(composer, payload);

            next.Compose(composer, payload);
        }

        void Stack<Vane<T>>.Push(Vane<T> vane)
        {
            lock (_vanes)
                _vanes.Push(vane);
        }

        void Stack<Vane<T>>.Pop(Vane<T> vane)
        {
            lock (_vanes)
            {
                if (_vanes.Peek() == vane)
                    _vanes.Pop();
            }
        }

        void Stack<Vane<T>>.Set(params Vane<T>[] vanes)
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