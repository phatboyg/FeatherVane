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
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;


    public class ModelSourceVane :
        SourceVane<IModel>
    {
        readonly SourceVane<IConnection> _connectionSourceVane;

        public ModelSourceVane(SourceVane<IConnection> connectionSourceVane)
        {
            _connectionSourceVane = connectionSourceVane;
        }

        public void Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, IModel>> next)
        {
            composer.Execute(() =>
                {
                    var get = new CreateModel<TPayload>(next);

                    return TaskComposer.Compose(_connectionSourceVane, payload, get, composer.CancellationToken);
                });
        }


        class CreateModel<TPayload> :
            Vane<Tuple<TPayload, IConnection>>
        {
            readonly Vane<Tuple<TPayload, IModel>> _next;

            internal CreateModel(Vane<Tuple<TPayload, IModel>> next)
            {
                _next = next;
            }

            void Vane<Tuple<TPayload, IConnection>>.Compose(Composer composer,
                Payload<Tuple<TPayload, IConnection>> payload)
            {
                IModel model = null;

                composer.Execute(() =>
                    {
                        IConnection connection = payload.Data.Item2;
                        model = CreateAndConfigureModel(connection);

                        Payload<TPayload> leftPayload = payload.SplitLeft();

                        return TaskComposer.Compose(_next, leftPayload.MergeRight(model), composer.CancellationToken);
                    });

                composer.Finally(() => CloseModel(model));
            }

            static void CloseModel(IModel model)
            {
                if (model != null)
                {

                    model.Close();
                    model.Dispose();
                }
            }

            IModel CreateAndConfigureModel(IConnection connection)
            {
                IModel model = connection.CreateModel();

                model.ConfirmSelect();
                model.BasicAcks += HandleAcks;
                model.BasicNacks += HandleNacks;
                model.BasicReturn += HandleReturns;

                return model;
            }

            void HandleReturns(IModel model, BasicReturnEventArgs args)
            {
                throw new NotImplementedException();
            }

            void HandleNacks(IModel model, BasicNackEventArgs args)
            {
                throw new NotImplementedException();
            }

            void HandleAcks(IModel model, BasicAckEventArgs args)
            {
                throw new NotImplementedException();
            }
        }
    }
}