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
    using Internals.Caching;
    using Internals.Extensions;

    /// <summary>
    /// Keep track of context types, keeping logic separate from the VaneContext where
    /// it is used
    /// </summary>
    public class ContextCache
    {
        readonly Cache<Type, CachedContext> _contextCache;

        public ContextCache()
        {
            _contextCache = new ConcurrentCache<Type, CachedContext>();
        }

        public bool HasContext(Type contextType)
        {
            return _contextCache.Has(contextType);
        }

        public bool TryGetContext<TContext>(out TContext context)
            where TContext : class
        {
            CachedContext cachedContext;
            if (_contextCache.TryGetValue(typeof(TContext), out cachedContext))
            {
                context = cachedContext.GetContext<TContext>();
                return true;
            }

            context = default(TContext);
            return false;
        }

        public TContext GetContext<TContext>(MissingContextProvider<TContext> missingContextProvider)
            where TContext : class
        {
            CachedContext cachedContext;
            if (_contextCache.TryGetValue(typeof(TContext), out cachedContext))
            {
                return cachedContext.GetContext<TContext>();
            }

            TContext context = missingContextProvider();
            if (context != default(TContext))
            {
                _contextCache.Add(typeof(TContext), new CachedContext<TContext>(context));
                return context;
            }

            throw new ArgumentException("The specified context was not found: " + typeof(TContext).GetTypeName(),
                "TContext");
        }

        interface CachedContext
        {
            T GetContext<T>()
                where T : class;
        }

        class CachedContext<TContext> :
            CachedContext
            where TContext : class
        {
            readonly TContext _context;

            public CachedContext(TContext context)
            {
                _context = context;
            }

            public T GetContext<T>()
                where T : class
            {
                var cachedContext = this as CachedContext<T>;
                if (cachedContext != null)
                    return cachedContext._context;

                throw new ArgumentException("Unexpected context type specified: " + typeof(TContext).GetTypeName(), "T");
            }
        }
    }
}