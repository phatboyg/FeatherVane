﻿// Copyright 2012-2013 Chris Patterson
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
namespace FeatherVane.VaneBuilders
{
    public interface VaneBuilder<T>
    {
        /// <summary>
        /// Appends a FeatherVane to the list of FeatherVanes for the Vane. FeatherVanes within a
        /// Vane are composed in the order in which they are added
        /// </summary>
        /// <param name="feather">The FeatherVane to be added</param>
        void Add(Feather<T> feather);
    }
}