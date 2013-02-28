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
namespace FeatherVane.Payloads
{
    using System;

    public class PayloadImpl<T> :
        Payload<T>
    {
        readonly PayloadContextCache _contextCache;
        readonly T _data;

        public PayloadImpl(T data)
        {
            _data = data;
            _contextCache = new PayloadContextCache();
        }

        public Type DataType
        {
            get { return typeof(T); }
        }

        public Type VaneType
        {
            get { return typeof(Feather<T>); }
        }

        public bool Has(Type contextType)
        {
            return _contextCache.HasContext(contextType);
        }

        public bool TryGet<TContext>(out TContext context)
            where TContext : class
        {
            return _contextCache.TryGetContext(out context);
        }

        public TContext GetOrAdd<TContext>(ContextFactory<TContext> contextFactory)
            where TContext : class
        {
            return _contextCache.GetContext(contextFactory);
        }

        public T Data
        {
            get { return _data; }
        }
    }
}