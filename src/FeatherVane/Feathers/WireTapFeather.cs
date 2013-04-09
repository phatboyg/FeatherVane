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
namespace FeatherVane.Feathers
{
    using Taskell;


    /// <summary>
    /// A WireTap passes the context to another Vane so that it can be observed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WireTapFeather<T> :
        Feather<T>,
        AcceptVaneVisitor
    {
        readonly Vane<T> _tap;

        public WireTapFeather(Vane<T> tap)
        {
            _tap = tap;
        }

        bool AcceptVaneVisitor.Accept(VaneVisitor visitor)
        {
            return visitor.Visit(_tap);
        }

        void Feather<T>.Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            composer.Execute(() => CompositionExtensions.Compose(_tap, payload, composer.CancellationToken));

            next.Compose(composer, payload);
        }
    }
}