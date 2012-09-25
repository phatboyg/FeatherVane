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
    using FeatherVane;

    /// <summary>
    /// Used as a Payload of a body type different than the original, but still retaining
    /// the original context for all related operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegatingPayload<T> :
        Payload<T>
        where T : class
    {
        readonly T _data;
        readonly Payload _originalPayload;

        public DelegatingPayload(Payload originalPayload, T data)
        {
            _originalPayload = originalPayload;
            _data = data;
        }

        public Type DataType
        {
            get { return typeof(T); }
        }

        public Type VaneType
        {
            get { return typeof(FeatherVane<T>); }
        }

        public bool Has(Type contextType)
        {
            return _originalPayload.Has(contextType);
        }

        public bool TryGet<TContext>(out TContext context)
            where TContext : class
        {
            return _originalPayload.TryGet(out context);
        }

        public TContext GetOrAdd<TContext>(ContextFactory<TContext> contextFactory)
            where TContext : class
        {
            return _originalPayload.GetOrAdd(contextFactory);
        }

        public T Data
        {
            get { return _data; }
        }
    }
}