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
    using Execution;
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
        FeatherVane<T>
        where T : class
    {
        readonly Func<Payload<T>, Type> _typeSelector;
        readonly Cache<Type, Vane<T>> _typeVanes;

        public TypeRouter(Func<Payload<T>, Type> typeSelector)
        {
            _typeSelector = typeSelector;
            _typeVanes = new ConcurrentCache<Type, Vane<T>>();
        }

        public Agenda<T> Plan(Planner<T> planner, Payload<T> payload, Vane<T> next)
        {
            Type contextType = _typeSelector(payload);

            Vane<T> typeVane = _typeVanes.Get(contextType, x => next);

            return typeVane.Plan(planner, payload);
        }

        public void Add<TOutput>(Vane<TOutput> nextVane, Func<Payload<T>, Payload<TOutput>> converter)
            where TOutput : class
        {
            _typeVanes.Add(typeof(TOutput), new TypeConverter<TOutput>(nextVane, converter));
        }

        class TypeConverter<TOutput> :
            Vane<T>
            where TOutput : class
        {
            readonly Func<Payload<T>, Payload<TOutput>> _converter;
            readonly Vane<TOutput> _vane;

            public TypeConverter(Vane<TOutput> vane, Func<Payload<T>, Payload<TOutput>> converter)
            {
                _vane = vane;
                _converter = converter;
            }

            public Agenda<T> Plan(Planner<T> planner, Payload<T> payload)
            {
                Payload<TOutput> output = _converter(payload);

                var outputPlanner = new AgendaPlanner<TOutput>();

                Agenda<TOutput> outputAgenda = _vane.Plan(outputPlanner, output);

                planner.Add(new TypeConverterAgendaItem(outputAgenda));

                return planner.CreateAgenda(payload);
            }

            class TypeConverterAgendaItem :
                AgendaItem<T>
            {
                readonly Agenda<TOutput> _agenda;

                public TypeConverterAgendaItem(Agenda<TOutput> agenda)
                {
                    _agenda = agenda;
                }

                public bool Execute(Agenda<T> agenda)
                {
                    return _agenda.Execute();
                }

                public bool Compensate(Agenda<T> agenda)
                {
                    return _agenda.Compensate();
                }
            }
        }
    }
}