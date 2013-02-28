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
namespace FeatherVane.RabbitMQIntegration.SourceVanes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RabbitMQ.Client;


    /// <summary>
    /// Connect a RabbitMQ connection
    /// </summary>
    public class ConnectSourceVane :
        SourceVane<IConnection>
    {
        readonly ConnectionFactory _connectionFactory;

        public ConnectSourceVane(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        static IEnumerable<int> Timeouts
        {
            get
            {
                yield return 0;
                yield return 100;
                yield return 1000;
                yield return 2000;
                yield return 5000;
                yield return 10000;
                while (true)
                    yield return 30000;
            }
        }

        public void Compose<TPayload>(Composer composer, Payload<TPayload> payload,
            Vane<Tuple<TPayload, IConnection>> next)
        {
            IEnumerator<int> timeoutEnumerator = null;
            composer.Execute(() => timeoutEnumerator = Timeouts.GetEnumerator());

            Func<Task> connectTask = null;
            connectTask = () => TaskComposer.Compose<IConnection>(composer.CancellationToken, x =>
                {
                    timeoutEnumerator.MoveNext();
                    int reconnectDelay = timeoutEnumerator.Current;

                    x.Delay(reconnectDelay);

                    IConnection connection = _connectionFactory.CreateConnection();

                    x.Compensate(compensation => compensation.Task(connectTask()));

                    Payload<Tuple<TPayload, IConnection>> nextPayload = payload.MergeRight(connection);
                    next.Compose(x, nextPayload);
                });

            composer.Execute(connectTask);

            composer.Finally(() => timeoutEnumerator.Dispose());
        }
    }
}