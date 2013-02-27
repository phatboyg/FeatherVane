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


    public struct Queue
    {
        public readonly IDictionary Arguments;
        public readonly bool AutoDelete;
        public readonly bool Durable;
        public readonly bool Exclusive;
        public readonly string Name;

        public Queue(string name, bool durable, bool exclusive, bool autoDelete, IDictionary arguments)
        {
            Name = name;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = arguments;
            Exclusive = exclusive;
        }

        public Queue(string name)
            : this()
        {
            Name = name;
        }
    }
}