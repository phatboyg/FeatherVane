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
    using System;
    using Util;


    public class VaneContextImpl<T> :
        VaneContext<T>
        where T : class
    {
        readonly ContextCache _contextCache;
        readonly T _body;

        public VaneContextImpl(T body)
        {
            _body = body;
            _contextCache = new ContextCache();
        }

        public Type ContextType
        {
            get { return typeof(T); }
        }

        public bool HasContext(Type contextType)
        {
            return _contextCache.HasContext(contextType);
        }

        public bool TryGetContext<TContext>(out TContext context)
            where TContext : class
        {
            return _contextCache.TryGetContext(out context);
        }

        public TContext GetContext<TContext>(MissingContextProvider<TContext> missingContextProvider)
            where TContext : class
        {
            return _contextCache.GetContext(missingContextProvider);
        }

        public T Body
        {
            get { return _body; }
        }
    }
}