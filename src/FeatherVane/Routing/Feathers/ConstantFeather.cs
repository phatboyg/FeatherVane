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
namespace FeatherVane.Routing.Feathers
{
    using Taskell;


    public class ConstantFeather :
        Feather<RoutingContext>
    {
        readonly int _id;

        public ConstantFeather(int id)
        {
            _id = id;
        }

        public void Compose(Composer composer, Payload<RoutingContext> payload, Vane<RoutingContext> next)
        {
            composer.Execute(() =>
                {
                    // we don't want to return the Task here, we just want to activate and continue
                    payload.Data.Activate(_id);
                });

            next.Compose(composer, payload);
        }
    }
}