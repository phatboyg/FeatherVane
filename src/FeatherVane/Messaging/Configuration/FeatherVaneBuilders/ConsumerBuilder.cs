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
namespace FeatherVane.Messaging.FeatherVaneBuilders
{
    using System.Collections.Generic;
    using System.Linq;
    using FeatherVane.FeatherVaneBuilders;
    using FeatherVane.Vanes;


    public class ConsumerBuilder<T> :
        FeatherVaneBuilder<Message>
    {
        readonly IList<FeatherVaneBuilder<Message>> _consumerConfigurators;
        readonly SourceVaneFactory<T> _sourceVaneFactory;

        public ConsumerBuilder(SourceVaneFactory<T> sourceVaneFactory,
            IList<FeatherVaneBuilder<Message>> consumerConfigurators)
        {
            _sourceVaneFactory = sourceVaneFactory;
            _consumerConfigurators = consumerConfigurators;
        }

        FeatherVane<Message> FeatherVaneBuilder<Message>.Build()
        {
            if (_consumerConfigurators.Count == 0)
                return new Shunt<Message>();

            IEnumerable<FeatherVane<Message>> featherVanes = _consumerConfigurators.Select(x => x.Build());

            return new Fanout<Message>(featherVanes);
        }
    }
}