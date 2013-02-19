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
    using System;
    using System.Collections.Generic;


    public class DictionaryVane :
        FeatherVane<Tuple<RoutingContext, string>>
    {
        readonly IDictionary<string, Vane<Tuple<RoutingContext, string>>> _vanes;

        public DictionaryVane(IDictionary<string, Vane<Tuple<RoutingContext, string>>> vanes)
        {
            _vanes = vanes;
        }

        public void Compose(Composer composer, Payload<Tuple<RoutingContext, string>> payload,
            Vane<Tuple<RoutingContext, string>> next)
        {
            composer.Execute(() =>
                {
                    Vane<Tuple<RoutingContext, string>> matchingVane;
                    if (_vanes.TryGetValue(payload.Data.Item2, out matchingVane))
                        return TaskComposer.Compose(matchingVane, payload, composer.CancellationToken);

                    return TaskUtil.Completed();
                });

            next.Compose(composer, payload);
        }
    }
}