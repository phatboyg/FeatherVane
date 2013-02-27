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


    public struct QueueBinding
    {
        public readonly IDictionary Arguments;
        public readonly Queue Queue;
        public readonly string RoutingKey;
        public readonly Exchange Exchange;

        public QueueBinding(Queue queue, Exchange exchange)
        {
            Exchange = exchange;
            Queue = queue;
            RoutingKey = "";
            Arguments = null;
        }

        public QueueBinding(Queue queue, Exchange exchange, IDictionary arguments)
        {
            Exchange = exchange;
            Queue = queue;
            Arguments = arguments;
            RoutingKey = "";
        }

        public QueueBinding(Queue queue, Exchange exchange, string routingKey)
        {
            Exchange = exchange;
            Queue = queue;
            RoutingKey = routingKey;
            Arguments = null;
        }

        public QueueBinding(Queue queue, Exchange exchange, string routingKey, IDictionary arguments)
        {
            Exchange = exchange;
            Queue = queue;
            RoutingKey = routingKey;
            Arguments = arguments;
        }

        public bool Equals(QueueBinding other)
        {
            return Queue.Equals(other.Queue) && string.Equals(RoutingKey, other.RoutingKey)
                   && Exchange.Equals(other.Exchange);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is QueueBinding && Equals((QueueBinding)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Queue.GetHashCode();
                hashCode = (hashCode * 397) ^ (RoutingKey != null
                                                   ? RoutingKey.GetHashCode()
                                                   : 0);
                hashCode = (hashCode * 397) ^ Exchange.GetHashCode();
                return hashCode;
            }
        }
    }
}