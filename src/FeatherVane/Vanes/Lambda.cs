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

    public class LambdaAction<T> :
        Vane<T>
        where T : class
    {
        readonly Action<VaneContext<T>> _handler;

        public LambdaAction(Action<VaneContext<T>> handler)
        {
            _handler = handler;
        }

        public VaneHandler<T> GetHandler(VaneContext<T> context, NextVane<T> next)
        {
            return new LambdaActionHandler(_handler);
        }

        class LambdaActionHandler :
            VaneHandler<T>
        {
            readonly Action<VaneContext<T>> _handler;

            public LambdaActionHandler(Action<VaneContext<T>> handler)
            {
                _handler = handler;
            }

            public void Handle(VaneContext<T> context)
            {
                _handler(context);
            }
        }
    }
}