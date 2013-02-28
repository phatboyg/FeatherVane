namespace FeatherVane.Tests.Benchmarks
{
    using System;
    using System.Collections.Generic;
    using FeatherVane.Routing;
    using FeatherVane.Routing.Feathers;
    using FeatherVane.Routing.SourceVanes;
    using Feathers;
    using Vanes;


    public class ControllerActionRoutingThroughput :
        RoutingThroughput
    {
        Vane<RoutingContext> _vane;

        public ControllerActionRoutingThroughput()
        {
            Vane<RoutingContext> routeVane = VaneFactory.Success<RoutingContext>();

            var constant = new ConstantFeather(1);
            var vane0 = VaneFactory.New<RoutingContext>(x => x.Feather(() => constant));
            var join1 = new JoinFeather(2, 1, VaneFactory.Success<RoutingContext>());
            var vane1 = VaneFactory.New<RoutingContext>(x => x.Feather(() => join1));
            var join2 = new JoinFeather(3, 2, routeVane);
            var vane2 = VaneFactory.New<RoutingContext>(x => x.Feather(() => join2));

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

            var segment2 = new SegmentSourceVane(2);

            var dictionary2 = new Dictionary<string, Vane<Tuple<RoutingContext, string>>>();
            dictionary2.Add("List", new LeftVane<RoutingContext, string>(vane2));

            var dictionaryVane2 = VaneFactory.New<Tuple<RoutingContext, string>>(x =>
                                                                                 x.Feather(() => new DictionaryFeather(dictionary2)));

            var segment2Vane = new SpliceSourceFeather<RoutingContext, string>(dictionaryVane2, segment2);

            _vane = VaneFactory.New<RoutingContext>(x =>
                {
                    x.Feather(() => segment0Vane);
                    x.Feather(() => segment1Vane);
                    x.Feather(() => segment2Vane);
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