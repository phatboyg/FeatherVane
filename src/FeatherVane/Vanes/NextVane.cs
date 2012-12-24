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
namespace FeatherVane.Vanes
{
    /// <summary>
    /// Curries a FeatherVane into a Vane, using the next Vane supplied during
    /// Build operations.
    /// </summary>
    /// <typeparam name="T">The Vane type</typeparam>
    public class NextVane<T> :
        Vane<T>,
        AcceptVaneVisitor
    {
        readonly FeatherVane<T> _featherVane;
        readonly Vane<T> _nextVane;

        /// <summary>
        /// Constructs a NextVane
        /// </summary>
        /// <param name="featherVane">The FeatherVane to combine with the next Vane</param>
        /// <param name="nextVane">The next Vane</param>
        public NextVane(FeatherVane<T> featherVane, Vane<T> nextVane)
        {
            _featherVane = featherVane;
            _nextVane = nextVane;
        }

        bool AcceptVaneVisitor.Accept(VaneVisitor visitor)
        {
            return visitor.Visit(_featherVane, x => visitor.Visit(_nextVane));
        }

        void Vane<T>.Compose(Composer composer, Payload<T> payload)
        {
            _featherVane.Compose(composer, payload, _nextVane);
        }
    }
}