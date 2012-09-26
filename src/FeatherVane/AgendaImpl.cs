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
    using System.Collections.Generic;

    public class AgendaImpl<T> :
        Agenda<T>
        where T : class
    {
        readonly IList<Exception> _exceptions;
        readonly IList<AgendaItem<T>> _items;
        readonly Payload<T> _payload;
        int _index = -1;

        public AgendaImpl(IEnumerable<AgendaItem<T>> agendaItems, Payload<T> payload)
        {
            _items = new List<AgendaItem<T>>(agendaItems);
            _exceptions = new List<Exception>();

            _payload = payload;
        }

        public bool IsCompleted
        {
            get { return _index + 1 == _items.Count; }
        }

        public bool IsExecuting
        {
            get { return _index > 0; }
        }

        public bool Execute()
        {
            if (IsCompleted)
                return true;

            AgendaItem<T> agendaItem = _items[++_index];
            try
            {
                bool result = agendaItem.Execute(this);
                if (result)
                    return true;
            }
            catch (Exception ex)
            {
                _exceptions.Add(ex);
            }

            Compensate();

            return false;
        }

        public bool Compensate()
        {
            if (!IsExecuting)
                throw new AgendaExecutionException(_exceptions).Flatten();

            AgendaItem<T> agendaItem = _items[--_index];
            try
            {
                return agendaItem.Compensate(this);
            }
            catch (Exception ex)
            {
                _exceptions.Add(ex);
                return Compensate();
            }
        }

        public Payload<T> Payload
        {
            get { return _payload; }
        }
    }
}