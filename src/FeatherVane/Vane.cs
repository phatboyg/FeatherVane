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
    public interface Vane<T>
        where T : class
    {
        Handler<T> GetHandler(Payload<T> context);
    }

    public static class Vane
    {
        public static Vane<T> Connect<T>(Vane<T> last, params FeatherVane<T>[] vanes) where T : class
        {
            Vane<T> next = last;
            for (int i = vanes.Length - 1; i >= 0; i--)
            {
                next = vanes[i].ConnectTo(next);
            }

            return next;
        }

        public static Vane<T> Connect<T>(FeatherVane<T> vane, Vane<T> next) where T : class
        {
            return new ConnectVane<T>(vane, next);
        }

        class ConnectVane<T> :
            Vane<T>
            where T : class
        {
            readonly Vane<T> _nextVane;
            readonly FeatherVane<T> _vane;

            public ConnectVane(FeatherVane<T> vane, Vane<T> nextVane)
            {
                _vane = vane;
                _nextVane = nextVane;
            }

            public Handler<T> GetHandler(Payload<T> context)
            {
                return _vane.GetHandler(context, _nextVane);
            }
        }
    }
}