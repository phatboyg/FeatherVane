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
    using Configurators;
    using FeatherVane.FeatherVaneBuilders;
    using FeatherVane.Vanes;


    public class ConsumerBuilder<T> :
        FeatherVaneBuilder<Message>
    {
        readonly IList<VaneBuilderConfigurator<Message>> _vaneConfigurators;

        public ConsumerBuilder(IList<VaneBuilderConfigurator<Message>> vaneConfigurators)
        {
            _vaneConfigurators = vaneConfigurators;
        }

        FeatherVane<Message> FeatherVaneBuilder<Message>.Build()
        {
            if (_vaneConfigurators.Count == 0)
                return new Shunt<Message>();

            var fanoutBuilder = new FanoutBuilder<Message>();

            foreach (var configurator in _vaneConfigurators)
                configurator.Configure(fanoutBuilder);

            return ((FeatherVaneBuilder<Message>)fanoutBuilder).Build();
        }
    }
}