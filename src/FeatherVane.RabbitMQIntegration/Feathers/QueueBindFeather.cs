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
namespace FeatherVane.RabbitMQIntegration.Feathers
{
    using RabbitMQ.Client;
    using Taskell;


    public class QueueBindFeather :
        Feather<IConnection>
    {
        readonly QueueBindings _bindings;

        public QueueBindFeather(QueueBindings bindings)
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

                        foreach (Queue queue in _bindings.Queues)
                        {
                            model.QueueDeclare(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete,
                                queue.Arguments);
                        }


                        foreach (QueueBinding binding in _bindings)
                        {
                            model.QueueBind(binding.Queue.Name, binding.Exchange.Name, binding.RoutingKey,
                                binding.Arguments);
                        }
                    }
                });
        }
    }
}