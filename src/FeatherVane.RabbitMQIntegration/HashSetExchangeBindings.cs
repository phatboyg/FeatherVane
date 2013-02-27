// Copyright 2012-2013 Chris Patterson
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
namespace FeatherVane.RabbitMQIntegration
{
    using System.Collections;
    using System.Collections.Generic;


    public class HashSetExchangeBindings :
        ExchangeBindings
    {
        readonly HashSet<ExchangeBinding> _bindings;
        readonly HashSet<Exchange> _exchanges;

        public HashSetExchangeBindings(params ExchangeBinding[] bindings)
        {
            _bindings = new HashSet<ExchangeBinding>(bindings);
            _exchanges = new HashSet<Exchange>();

            foreach (ExchangeBinding binding in bindings)
            {
                _exchanges.Add(binding.Source);
                _exchanges.Add(binding.Destination);
            }
        }

        public IEnumerable<Exchange> Exchanges
        {
            get { return _exchanges; }
        }

        public IEnumerator<ExchangeBinding> GetEnumerator()
        {
            return _bindings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}