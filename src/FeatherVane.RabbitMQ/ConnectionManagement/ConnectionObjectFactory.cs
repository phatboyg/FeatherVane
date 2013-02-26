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
namespace FeatherVane.RabbitMQIntegration.ConnectionManagement
{
    using System;
    using RabbitMQ.Client;


    public class ConnectionObjectFactory :
        PooledObjectFactory<IConnection>
    {
        readonly ConnectionFactory _connectionFactory;

        public ConnectionObjectFactory(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IConnection New()
        {
            return _connectionFactory.CreateConnection();
        }

        public void Dispose(IConnection instance)
        {
            try
            {
                instance.Close();
            }
            catch
            {
            }
            finally
            {
                try
                {
                    instance.Dispose();
                }
                catch
                {
                }
            }
        }

        public void Activate(IConnection instance)
        {
        }

        public void Passivate(IConnection instance)
        {
        }

        public bool Validate(IConnection instance)
        {
            return instance.IsOpen;
        }
    }
}