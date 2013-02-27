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
namespace FeatherVane.RabbitMQIntegration.Vanes
{
    using RabbitMQ.Client;


    public class ExchangeBindVane :
        FeatherVane<IConnection>
    {
        readonly ExchangeBindings _bindings;

        public ExchangeBindVane(ExchangeBindings bindings)
        {
            _bindings = bindings;
        }

        public void Compose(Composer composer, Payload<IConnection> payload, Vane<IConnection> next)
        {
            composer.Execute(() =>
                {
                    IConnection connection = payload.Data;

                    using (IModel model = connection.CreateModel())
                    {
                        foreach (Exchange exchange in _bindings.Exchanges)
                        {
                            model.ExchangeDeclare(exchange.Name, exchange.Type, exchange.Durable, exchange.AutoDelete,
                                exchange.Arguments);
                        }

                        foreach (ExchangeBinding binding in _bindings)
                            model.ExchangeBind(binding.Destination.Name, binding.Source.Name, binding.RoutingKey);
                    }
                });
        }
    }
}