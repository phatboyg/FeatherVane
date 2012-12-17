﻿// Copyright 2012-2012 Chris Patterson
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

    public class ExecuteAction<T> :
        FeatherVane<T>
    {
        readonly Action<Payload<T>> _handler;

        public ExecuteAction(Action<Payload<T>> handler)
        {
            _handler = handler;
        }

        public void Build(Builder<T> builder, Payload<T> payload, Vane<T> next)
        {
            builder.Execute(() => _handler(payload));

            next.Build(builder, payload);
        }
    }
}