// Copyright 2012-2012 Chris Patterson
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
namespace FeatherVane.Visualization
{
    using System;

#if !NETFX_CORE
    [Serializable]
#endif
    public class Edge
    {
        public Edge(Vertex from, Vertex to, string title)
        {
            From = from;
            To = to;
            Title = title;
        }

        protected bool Equals(Edge other)
        {
            return Equals(To, other.To) && Equals(From, other.From);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((Edge)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((To != null
                             ? To.GetHashCode()
                             : 0) * 397) ^ (From != null
                                                ? From.GetHashCode()
                                                : 0);
            }
        }

        public Vertex To { get; private set; }

        public Vertex From { get; private set; }

        public string Title { get; private set; }
    }
}