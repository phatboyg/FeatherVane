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
    using Internals.Caching;

    /// <summary>
    /// The TypeRouter allows vanes to be registered for types that can be converted from the input
    /// context, allowing type-based content to be routed to a single vane.
    /// 
    /// Another type-based router should be the MultiTypeRouter, which enumerates the registered type
    /// handlers and gets zero or more interested type handlers for input context that can have multiple
    /// renderings
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypeRouter<T> :
        Vane<T>
        where T : class
    {
        readonly Func<VaneContext<T>, Type> _typeSelector;
        readonly Cache<Type, NextVane<T>> _typeVanes;

        public TypeRouter(Func<VaneContext<T>, Type> typeSelector)
        {
            _typeSelector = typeSelector;
            _typeVanes = new ConcurrentCache<Type, NextVane<T>>();
        }

        public VaneHandler<T> GetHandler(VaneContext<T> context, NextVane<T> next)
        {
            Type contextType = _typeSelector(context);

            NextVane<T> typeVane = _typeVanes.Get(contextType, x => next);

            return typeVane.GetHandler(context);
        }

        public void Add<TOutput>(NextVane<TOutput> nextVane, Func<VaneContext<T>, VaneContext<TOutput>> converter) 
            where TOutput : class
        {
            _typeVanes.Add(typeof(TOutput), new TypeConverter<T, TOutput>(nextVane, converter));
        }

        class TypeConverter<T, TOutput> :
            NextVane<T>
            where T : class
            where TOutput : class
        {
            readonly Func<VaneContext<T>, VaneContext<TOutput>> _converter;
            readonly NextVane<TOutput> _vane;

            public TypeConverter(NextVane<TOutput> vane, Func<VaneContext<T>, VaneContext<TOutput>> converter)
            {
                _vane = vane;
                _converter = converter;
            }

            public VaneHandler<T> GetHandler(VaneContext<T> context)
            {
                VaneContext<TOutput> output = _converter(context);

                VaneHandler<TOutput> handler = _vane.GetHandler(output);

                return new TypeConverterHandler(handler, _converter);
            }

            class TypeConverterHandler :
                VaneHandler<T>
            {
                readonly Func<VaneContext<T>, VaneContext<TOutput>> _converter;
                readonly VaneHandler<TOutput> _handler;

                public TypeConverterHandler(VaneHandler<TOutput> handler, Func<VaneContext<T>, VaneContext<TOutput>> converter)
                {
                    _handler = handler;
                    _converter = converter;
                }

                public void Handle(VaneContext<T> context)
                {
                    VaneContext<TOutput> output = _converter(context);

                    _handler.Handle(output);
                }
            }
        }
    }
}