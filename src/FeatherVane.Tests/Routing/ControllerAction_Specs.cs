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
namespace FeatherVane.Tests.Routing
{
    using System;
    using System.Collections.Generic;
    using FeatherVane.Routing;
    using FeatherVane.Routing.Vanes;
    using NUnit.Framework;
    using Vanes;


    [TestFixture]
    public class ControllerAction_Specs
    {
        [Test]
        public void Should_route_a_controller_action()
        {
            var uri = new Uri("http://localhost/Accounts/List");

            var routeTestVane = new RouteTestVane();
            Vane<RoutingContext> routeVane = VaneFactory.New<RoutingContext>(x => x.FeatherVane(() => routeTestVane));


            var constant = new ConstantVane(1);
            var vane0 = VaneFactory.New<RoutingContext>(x => x.FeatherVane(() => constant));
            var join1 = new JoinVane(2, 1, VaneFactory.Success<RoutingContext>());
            var vane1 = VaneFactory.New<RoutingContext>(x => x.FeatherVane(() => join1));
            var join2 = new JoinVane(3, 2, routeVane);
            var vane2 = VaneFactory.New<RoutingContext>(x => x.FeatherVane(() => join2));






            var segment0 = new SegmentVane(0);

            var dictionary0 = new Dictionary<string, Vane<Tuple<RoutingContext, string>>>();
            dictionary0.Add("/", new LeftVane<RoutingContext, string>(vane0));

            var dictionaryVane0 = VaneFactory.New<Tuple<RoutingContext, string>>(x =>
                x.FeatherVane(() => new DictionaryVane(dictionary0)));

            var segment0Vane = new SpliceSourceVane<RoutingContext, string>(dictionaryVane0, segment0);



            var segment1 = new SegmentVane(1);

            var dictionary1 = new Dictionary<string, Vane<Tuple<RoutingContext, string>>>();
            dictionary1.Add("Accounts/", new LeftVane<RoutingContext, string>(vane1));

            var dictionaryVane1 = VaneFactory.New<Tuple<RoutingContext, string>>(x => 
                x.FeatherVane(() => new DictionaryVane(dictionary1)));

            var segment1Vane = new SpliceSourceVane<RoutingContext, string>(dictionaryVane1, segment1);

  var segment2 = new SegmentVane(2);

            var dictionary2 = new Dictionary<string, Vane<Tuple<RoutingContext, string>>>();
            dictionary2.Add("List", new LeftVane<RoutingContext, string>(vane2));

            var dictionaryVane2 = VaneFactory.New<Tuple<RoutingContext, string>>(x => 
                x.FeatherVane(() => new DictionaryVane(dictionary2)));

            var segment2Vane = new SpliceSourceVane<RoutingContext, string>(dictionaryVane2, segment2);

            var vane = VaneFactory.New<RoutingContext>(x =>
                {
                    x.FeatherVane(() => segment0Vane);
                    x.FeatherVane(() => segment1Vane);
                    x.FeatherVane(() => segment2Vane);
                });


            var context = new RoutingContextImpl(uri);
            vane.Execute(context);
            context.Complete();


            Assert.IsTrue(routeTestVane.ComposeCalled);
            Assert.IsTrue(routeTestVane.ExecuteCalled);
            Assert.IsNotNull(routeTestVane.RoutingContext);
        }
    }
}