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
namespace FeatherVane.Tests.Benchmarks
{
    using System;
    using System.Collections.Generic;
    using FeatherVane.Routing;
    using FeatherVane.Routing.Feathers;
    using FeatherVane.Routing.SourceVanes;
    using Feathers;
    using Vanes;


    public class ControllerRoutingThroughput :
        RoutingThroughput
    {
        Vane<RoutingContext> _vane;

        public ControllerRoutingThroughput()
        {
            Vane<RoutingContext> routeVane = VaneFactory.Success<RoutingContext>();

            var constant = new ConstantFeather(1);
            var vane0 = VaneFactory.New<RoutingContext>(x => x.Feather(() => constant));
            var join1 = new JoinFeather(2, 1, routeVane);
            var vane1 = VaneFactory.New<RoutingContext>(x => x.Feather(() => join1));

            var segment0 = new SegmentSourceVane(0);

            var dictionary0 = new Dictionary<string, Vane<Tuple<RoutingContext, string>>>();
            dictionary0.Add("/", new LeftVane<RoutingContext, string>(vane0));

            var dictionaryVane0 = VaneFactory.New<Tuple<RoutingContext, string>>(x =>
                x.Feather(() => new DictionaryFeather(dictionary0)));

            var segment0Vane = new SpliceSourceFeather<RoutingContext, string>(dictionaryVane0, segment0);

            var segment1 = new SegmentSourceVane(1);

            var dictionary1 = new Dictionary<string, Vane<Tuple<RoutingContext, string>>>();
            dictionary1.Add("Accounts/", new LeftVane<RoutingContext, string>(vane1));

            var dictionaryVane1 = VaneFactory.New<Tuple<RoutingContext, string>>(x =>
                x.Feather(() => new DictionaryFeather(dictionary1)));

            var segment1Vane = new SpliceSourceFeather<RoutingContext, string>(dictionaryVane1, segment1);

            _vane = VaneFactory.New<RoutingContext>(x =>
                {
                    x.Feather(() => segment0Vane);
                    x.Feather(() => segment1Vane);
                });

        }

        public void Route(Uri uri)
        {
            var context = new RoutingContextImpl(uri);
            _vane.Execute(context);
            context.Complete();
        }
    }
}