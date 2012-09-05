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
    using Internals.Caching;
    using Internals.Extensions;

    /// <summary>
    /// Keep track of context types, keeping logic separate from the VaneContext where
    /// it is used
    /// </summary>
    public class PayloadContextCache
    {
        readonly Cache<Type, CachedContext> _cache;

        public PayloadContextCache()
        {
            _cache = new ConcurrentCache<Type, CachedContext>();
        }

        public bool HasContext(Type contextType)
        {
            return _cache.Has(contextType);
        }

        public bool TryGetContext<TContext>(out TContext context)
            where TContext : class
        {
            CachedContext cachedContext;
            if (_cache.TryGetValue(typeof(TContext), out cachedContext))
            {
                context = cachedContext.GetContext<TContext>();
                return true;
            }

            context = default(TContext);
            return false;
        }

        public TContext GetContext<TContext>(ContextFactory<TContext> contextFactory)
            where TContext : class
        {
            CachedContext cachedContext;
            if (_cache.TryGetValue(typeof(TContext), out cachedContext))
            {
                return cachedContext.GetContext<TContext>();
            }

            try
            {
                TContext context = contextFactory();
                if (context != default(TContext))
                {
                    _cache.Add(typeof(TContext), new CachedContext<TContext>(context));
                    return context;
                }
            }
            catch (Exception ex)
            {
                throw new ContextFactoryException("The context factory threw an exception creating a missing context: "
                                                  + typeof(TContext).GetTypeName(), ex);
            }

            throw new ContextNotFoundException("The request context was not found and could not be created: "
                                               + typeof(TContext).GetTypeName());
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

                throw new InternalFeatherVaneException("The context type did not match the expected type: "
                                                       + typeof(TContext).GetTypeName());
            }
        }
    }
}