﻿// Copyright 2012-2013 Chris Patterson
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
namespace FeatherVane.Tests.Routing
{
    using System.Diagnostics;
    using FeatherVane.Routing;
    using Taskell;


    [DebuggerNonUserCode]
    public class RouteTest :
        Feather<RoutingContext>
    {
        public bool ComposeCalled { get; set; }
        public bool ExecuteCalled { get; set; }
        public RoutingContext RoutingContext { get; set; }

        public void Compose(Composer composer, Payload<RoutingContext> payload, Vane<RoutingContext> next)
        {
            ComposeCalled = true;

            composer.Execute(() =>
                {
                    RoutingContext = payload.Data;
                    ExecuteCalled = true;
                });

            next.Compose(composer, payload);
        }
    }
}