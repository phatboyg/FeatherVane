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
    using System;
    using System.Collections.Generic;
    using Configurators;
    using FeatherVane.FeatherVaneBuilders;
    using FeatherVane.Vanes;
    using VaneConfigurators;


    public class ConsumerBuilder<T> :
        FeatherVaneBuilder<Message>
    {
        readonly IList<VaneBuilderConfigurator<Tuple<Message, T>>> _consumerConfigurators;
        readonly SourceVaneFactory<T> _sourceVaneFactory;

        public ConsumerBuilder(SourceVaneFactory<T> sourceVaneFactory,
            IList<VaneBuilderConfigurator<Tuple<Message, T>>> consumerConfigurators)
        {
            _sourceVaneFactory = sourceVaneFactory;
            _consumerConfigurators = consumerConfigurators;
        }

        FeatherVane<Message> FeatherVaneBuilder<Message>.Build()
        {
            var vaneConfigurator = new SuccessConfigurator<Tuple<Message, T>>();
            if (_consumerConfigurators.Count == 0)
                vaneConfigurator.Fanout(x => { });
            else if (_consumerConfigurators.Count == 1)
                vaneConfigurator.Add(_consumerConfigurators[0]);
            else
            {
                vaneConfigurator.Fanout(x =>
                {
                    foreach (var configurator in _consumerConfigurators)
                        x.Add(configurator);
                });
            }
            Vane<Tuple<Message, T>> messageVane = vaneConfigurator.Create();
            SourceVane<T> sourceVane = _sourceVaneFactory.Create();

            var spliceVane = new Splice<Message, T>(messageVane, sourceVane);

            return spliceVane;
        }
    }
}