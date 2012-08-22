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
namespace FeatherVane.ContextUtils
{
    using System;

    /// <summary>
    /// Used as a VaneContext of a body type different than the original, but still retaining
    /// the original context for all related operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegatingVaneContext<T> :
        VaneContext<T>
        where T : class
    {
        readonly T _body;
        readonly VaneContext _originalContext;

        public DelegatingVaneContext(VaneContext originalContext, T body)
        {
            _originalContext = originalContext;
            _body = body;
        }

        public Type BodyType
        {
            get { return typeof(T); }
        }

        public Type VaneType
        {
            get { return typeof(Vane<T>); }
        }

        public bool Has(Type contextType)
        {
            return _originalContext.Has(contextType);
        }

        public bool TryGet<TContext>(out TContext context)
            where TContext : class
        {
            return _originalContext.TryGet(out context);
        }

        public TContext Get<TContext>(MissingContextProvider<TContext> missingContextProvider)
            where TContext : class
        {
            return _originalContext.Get(missingContextProvider);
        }

        public T Body
        {
            get { return _body; }
        }
    }
}