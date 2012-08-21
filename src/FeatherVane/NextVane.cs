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
namespace FeatherVane
{
    using Vanes;

    public interface NextVane<T>
    {
        VaneHandler<T> GetHandler(T context);
    }

    public static class NextVane
    {
        public static NextVane<T> Connect<T>(NextVane<T> last, params Vane<T>[] vanes)
        {
            NextVane<T> next = last;
            for (int i = vanes.Length - 1; i >= 0; i--)
            {
                next = vanes[i].ConnectTo(next);
            }

            return next;
        }

        public static NextVane<T> Connect<T>(Vane<T> vane, NextVane<T> next)
        {
            return new ConnectVane<T>(vane, next);
        }

        class ConnectVane<T> :
            NextVane<T>
        {
            readonly NextVane<T> _nextVane;
            readonly Vane<T> _vane;

            public ConnectVane(Vane<T> vane, NextVane<T> nextVane)
            {
                _vane = vane;
                _nextVane = nextVane;
            }

            public VaneHandler<T> GetHandler(T context)
            {
                return _vane.GetHandler(context, _nextVane);
            }
        }
    }
}