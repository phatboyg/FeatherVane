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


    public struct Exchange
    {
        public readonly IDictionary Arguments;
        public readonly bool AutoDelete;
        public readonly bool Durable;
        public readonly string Name;
        public readonly string Type;

        public Exchange(string name, string type, bool durable, bool autoDelete)
        {
            Name = name;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = null;
        }

        public Exchange(string name, string type, bool durable, bool autoDelete, IDictionary arguments)
        {
            Name = name;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = arguments;
        }

        public bool Equals(Exchange other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Exchange && Equals((Exchange)obj);
        }

        public override int GetHashCode()
        {
            return (Name != null
                        ? Name.GetHashCode()
                        : 0);
        }
    }
}