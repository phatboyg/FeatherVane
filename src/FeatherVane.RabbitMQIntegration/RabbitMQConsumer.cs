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
    using RabbitMQ.Client;


    public class RabbitMQConsumer :
        IBasicConsumer
    {
        public void HandleBasicConsumeOk(string consumerTag)
        {
            throw new System.NotImplementedException();
        }

        public void HandleBasicCancelOk(string consumerTag)
        {
            throw new System.NotImplementedException();
        }

        public void HandleBasicCancel(string consumerTag)
        {
            throw new System.NotImplementedException();
        }

        public void HandleModelShutdown(IModel model, ShutdownEventArgs reason)
        {
            throw new System.NotImplementedException();
        }

        public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, byte[] body)
        {
            throw new System.NotImplementedException();
        }

        public IModel Model
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}