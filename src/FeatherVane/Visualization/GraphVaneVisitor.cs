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
    using System.Collections.Generic;
    using Internals.Caching;
    using Internals.Extensions;
    using SourceVanes;
    using Vanes;


    public class GraphVaneVisitor :
        VaneVisitor
    {
        readonly HashSet<Edge> _edges = new HashSet<Edge>();
        readonly HashSet<object> _seen = new HashSet<object>();
        readonly Stack<Vertex> _stack = new Stack<Vertex>();
        readonly Cache<int, Vertex> _vertices = new DictionaryCache<int, Vertex>();
        Vertex _current;
        static VaneTypeNameFormatter _typeNameFormatter = new VaneTypeNameFormatter();

        public bool Visit<T>(Vane<T> vane)
        {
            VisitVane(vane);

            return true;
        }

        public bool Visit<T>(SourceVane<T> vane)
        {
            VisitSourceVane(vane);

            return true;
        }

        public bool Visit<T>(FeatherVane<T> vane)
        {
            VisitFeatherVane(vane);

            return true;
        }

        public FeatherVaneGraph GetGraphData()
        {
            return new FeatherVaneGraph(_vertices, _edges);
        }

        Vertex GetVertex(int key, Func<string> getTitle, Type nodeType, Type objectType)
        {
            return _vertices.Get(key, _ =>
                {
                    var newSink = new Vertex(nodeType, objectType, getTitle());

                    return newSink;
                });
        }

        Vertex GetVertex<T>(FeatherVane<T> vane)
        {
            return GetVertex(vane.GetHashCode(), () => GetTitle(vane.GetType()), vane.GetType(), typeof(T));
        }

        Vertex GetVertex<T>(SourceVane<T> vane)
        {
            return GetVertex(vane.GetHashCode(), () => GetTitle(vane.GetType()), vane.GetType(), typeof(T));
        }

        Vertex GetVertex<T>(Vane<T> vane)
        {
            return GetVertex(vane.GetHashCode(), () => GetTitle(vane.GetType()), vane.GetType(), typeof(T));
        }

        static string GetTitle(Type vaneType)
        {
            return _typeNameFormatter.GetTypeName(vaneType);
        }

        void VisitFeatherVane<T>(FeatherVane<T> vane)
        {
            if (_seen.Contains(vane))
                return;

            _current = GetVertex(vane);
            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

            _seen.Add(vane);

            Push(() => VisitAcceptor(vane));
        }

        void VisitSourceVane<T>(SourceVane<T> vane)
        {
            _seen.Add(vane);

            var nextSourceVane = vane as NextSource<T>;
            if (nextSourceVane != null)
            {
                VisitNextSourceVane(nextSourceVane);
                return;
            }

            _current = GetVertex(vane);
            if (_stack.Count > 0)
                _edges.Add(new Edge(_current, _stack.Peek(), _current.TargetType.Name));

            Push(() => VisitAcceptor(vane));
        }

        void VisitVane<T>(Vane<T> vane)
        {
            if (_seen.Contains(vane))
                return;

            _seen.Add(vane);

            var nextVane = vane as NextVane<T>;
            if (nextVane != null)
            {
                VisitNextVane(nextVane);
                return;
            }

            var successVane = vane as Success<T>;
            if (successVane != null)
                return;

            var unhandled = vane as Unhandled<T>;
            if (unhandled != null)
                return;

            _current = GetVertex(vane);

            if (_stack.Count > 0)
                _edges.Add(new Edge(_stack.Peek(), _current, _current.TargetType.Name));

        }

        void VisitNextVane<T>(NextVane<T> nextVane)
        {
            Visit(nextVane.FeatherVane);
            Push(() => Visit(nextVane.Next));
        }

        void VisitNextSourceVane<T>(NextSource<T> nextVane)
        {
            _current = GetVertex(nextVane.Next);
            if (_stack.Count > 0)
                _edges.Add(new Edge(_current, _stack.Peek(), _current.TargetType.Name));

            Push(() =>
                {
                    VisitAcceptor(nextVane.Next);
                    Visit(nextVane.Source);
                });
        }

        void Push(Action callback)
        {
            _stack.Push(_current);

            callback();

            _stack.Pop();
        }

        void VisitAcceptor(object vane)
        {
            var acceptVaneVisitor = vane as AcceptVaneVisitor;
            if (acceptVaneVisitor != null)
                acceptVaneVisitor.Accept(this);
        }
    }
}