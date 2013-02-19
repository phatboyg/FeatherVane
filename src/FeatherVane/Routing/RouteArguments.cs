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
namespace FeatherVane.Routing
{
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// The arguments that were matched to parameters in the route
    /// </summary>
    public interface RouteArguments :
        IEnumerable<RouteArgument>
    {
        int Count { get; }

        bool Has(string name);

        void Each(Action<RouteArgument> callback);

        void Each(Action<string, RouteArgument> callback);

        bool Exists(Predicate<RouteArgument> predicate);

        bool Find(Predicate<RouteArgument> predicate, out RouteArgument result);

        string[] GetAllKeys();

        RouteArgument[] GetAll();
    }
}