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
namespace FeatherVane.Messaging
{
    using System;
    using FeatherVaneConfigurators;
    using SourceVaneConfigurators;


    public static class InstanceConfigurationExtensions
    {
        public static void Instance<TInstance>(this VaneConfigurator<Message> configurator,
            TInstance instance, Action<ConsumerConfigurator<TInstance>> configureCallback)
        {
            var sourceVaneConfigurator = new SourceVaneConfiguratorImpl<TInstance>();
            sourceVaneConfigurator.Instance(instance);

            var consumerConfigurator = new ConsumerConfiguratorImpl<TInstance>(sourceVaneConfigurator);

            configureCallback(consumerConfigurator);

            configurator.Add(consumerConfigurator);
        }
    }
}