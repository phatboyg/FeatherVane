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

    public class DelegatingVaneContext<T> :
        VaneContext<T>
        where T : class
    {
        T _body;
        VaneContext _originalContext;

        public DelegatingVaneContext(VaneContext originalContext, T body)
        {
            _originalContext = originalContext;
            _body = body;
        }

        public Type ContextType
        {
            get { return typeof(T); }
        }

        public bool HasContext(Type contextType)
        {
            return _originalContext.HasContext(contextType);
        }

        public bool TryGetContext<TContext>(out TContext context)
            where TContext : class
        {
            return _originalContext.TryGetContext(out context);
        }

        public TContext GetContext<TContext>(MissingContextProvider<TContext> missingContextProvider)
            where TContext : class
        {
            return _originalContext.GetContext(missingContextProvider);
        }

        public T Body
        {
            get { return _body; }
        }
    }
}