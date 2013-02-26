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
    public struct ExchangeBinding
    {
        public readonly Exchange Destination;
        public readonly string RoutingKey;
        public readonly Exchange Source;

        public ExchangeBinding(Exchange destination, Exchange source)
        {
            Source = source;
            Destination = destination;
            RoutingKey = "";
        }

        public ExchangeBinding(Exchange destination, Exchange source, string routingKey)
        {
            Source = source;
            Destination = destination;
            RoutingKey = routingKey;
        }

        public bool Equals(ExchangeBinding other)
        {
            return Destination.Equals(other.Destination) && string.Equals(RoutingKey, other.RoutingKey)
                   && Source.Equals(other.Source);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is ExchangeBinding && Equals((ExchangeBinding)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Destination.GetHashCode();
                hashCode = (hashCode * 397) ^ (RoutingKey != null
                                                   ? RoutingKey.GetHashCode()
                                                   : 0);
                hashCode = (hashCode * 397) ^ Source.GetHashCode();
                return hashCode;
            }
        }
    }
}