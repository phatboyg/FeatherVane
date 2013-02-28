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
    using System.Collections.Generic;
    using System.Text;
    using Internals.Extensions;
    using Vanes;


    public class StringVaneVisitor :
        VaneVisitor
    {
        readonly StringBuilder _sb;
        readonly HashSet<object> _seen;
        int _depth;

        public StringVaneVisitor()
        {
            _sb = new StringBuilder();
            _seen = new HashSet<object>();
        }

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

        public bool Visit<T, TSource>(SourceVane<T, TSource> vane)
        {
            VisitSourceVane(vane);

            return true;
        }

        public bool Visit<T>(Feather<T> vane)
        {
            VisitFeatherVane(vane);

            return true;
        }

        void VisitVane<T>(Vane<T> vane)
        {
            if (_seen.Contains(vane))
                return;

            if (!(vane is NextVane<T>))
                Append(vane.GetType().GetTypeName());

            _seen.Add(vane);

            VisitAcceptor<T>(vane);
        }

        void VisitFeatherVane<T>(Feather<T> vane)
        {
            if (_seen.Contains(vane))
                return;

            Append(vane.GetType().GetTypeName());
            _seen.Add(vane);

            VisitAcceptor<T>(vane);
        }

        void VisitSourceVane<T>(SourceVane<T> vane)
        {
            if (_seen.Contains(vane))
                return;

            Append(vane.GetType().GetTypeName());
            _seen.Add(vane);

            VisitAcceptor<T>(vane);
        }

        void VisitSourceVane<T, TSource>(SourceVane<T, TSource> vane)
        {
            if (_seen.Contains(vane))
                return;

            Append(vane.GetType().GetTypeName());
            _seen.Add(vane);

            VisitAcceptor<T>(vane);
        }

        void Append(string text)
        {
            _sb.Append(' ', _depth * 2);
            _sb.AppendLine(text);
        }

        void VisitAcceptor<T>(object vane)
        {
            if (!(vane is NextVane<T>))
                _depth++;

            var acceptVaneVisitor = vane as AcceptVaneVisitor;
            if (acceptVaneVisitor != null)
                (acceptVaneVisitor).Accept(this);

            if (!(vane is NextVane<T>))
                _depth--;
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}