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
    using System;
    using ConnectionManagement;
    using RabbitMQ.Client;


    public class ConnectionSourceVane :
        SourceVane<IConnection>
    {
        readonly ObjectPool<IConnection> _connectionPool;

        public ConnectionSourceVane(ObjectPool<IConnection> connectionPool)
        {
            _connectionPool = connectionPool;
        }

        public void Compose<TPayload>(Composer composer, Payload<TPayload> payload,
            Vane<Tuple<TPayload, IConnection>> next)
        {
            IConnection connection = null;
            composer.Execute(() =>
                {
                    connection = _connectionPool.Acquire();
                    Payload<Tuple<TPayload, IConnection>> nextPayload = payload.MergeRight(connection);

                    return TaskComposer.Compose(next, nextPayload, composer.CancellationToken);
                });

            composer.Compensate(compensation =>
                {
                    connection.Close(500, "exception");
                    return compensation.Throw();
                });

            composer.Finally(() => _connectionPool.Surrender(connection));
        }
    }
}