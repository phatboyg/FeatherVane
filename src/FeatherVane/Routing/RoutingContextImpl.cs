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
    using System.Threading.Tasks;
    using Internals.Caching;


    public class RoutingContextImpl :
        RoutingContext
    {
        readonly Cache<int, Activation> _activations;
        readonly string[] _segments;
        readonly Uri _uri;

        public RoutingContextImpl(Uri uri)
        {
            _uri = uri;
            _segments = _uri.Segments;

            _activations = new ConcurrentCache<int, Activation>(x => x.Id);
        }

        public string Segment(int position)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position");

            if (position >= _segments.Length)
                return null;

            return _segments[position] ?? "";
        }

        public Task Activate(int id)
        {
            Activation activation = _activations.Get(id, x => new CompletedActivation(x));
            activation.Complete();

            return activation.Task;
        }

        public Task GetActivation(int id)
        {
            return _activations.Get(id, x => new PendingActivation(x)).Task;
        }

        public void Complete()
        {
            _activations.Each(x => x.Cancel());
        }
    }
}