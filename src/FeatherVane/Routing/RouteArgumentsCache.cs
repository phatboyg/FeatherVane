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
    using System.Collections;
    using System.Collections.Generic;
    using Internals.Caching;


    class RouteArgumentsCache :
        RouteArguments
    {
        readonly Cache<string, RouteArgument> _cache;

        internal RouteArgumentsCache(Cache<string, RouteArgument> cache)
        {
            _cache = cache;
        }

        public IEnumerator<RouteArgument> GetEnumerator()
        {
            return _cache.GetEnumerator();
        }

        public int Count
        {
            get { return _cache.Count; }
        }

        public bool Has(string key)
        {
            return _cache.Has(key);
        }

        public void Each(Action<RouteArgument> callback)
        {
            _cache.Each(callback);
        }

        public void Each(Action<string, RouteArgument> callback)
        {
            _cache.Each(callback);
        }

        public bool Exists(Predicate<RouteArgument> predicate)
        {
            return _cache.Exists(predicate);
        }

        public bool Find(Predicate<RouteArgument> predicate, out RouteArgument result)
        {
            return _cache.Find(predicate, out result);
        }

        public string[] GetAllKeys()
        {
            return _cache.GetAllKeys();
        }

        public RouteArgument[] GetAll()
        {
            return _cache.GetAll();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cache.GetEnumerator();
        }
    }
}