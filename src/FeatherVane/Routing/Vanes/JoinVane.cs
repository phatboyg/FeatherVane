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
namespace FeatherVane.Routing.Vanes
{
    public class JoinVane :
        FeatherVane<RoutingContext>
    {
        readonly int _id;
        readonly Vane<RoutingContext> _output;
        readonly int _rightId;

        public JoinVane(int id, int rightId, Vane<RoutingContext> output)
        {
            _id = id;
            _rightId = rightId;
            _output = output;
        }

        public void Compose(Composer composer, Payload<RoutingContext> payload, Vane<RoutingContext> next)
        {
            composer.Execute(() =>
                {
                    payload.Data.GetActivation(_rightId)
                           .Then(() => TaskUtil.RunSynchronously(() =>
                               {
                                   payload.Data.Activate(_id);

                                   _output.Compose(composer, payload);
                               }, composer.CancellationToken), composer.CancellationToken);
                });

            next.Compose(composer, payload);
        }
    }
}