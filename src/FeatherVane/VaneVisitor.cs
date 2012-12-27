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
namespace FeatherVane
{
    using System;


    public interface VaneVisitor
    {
        bool Visit<T>(Vane<T> vane);
        bool Visit<T>(Vane<T> vane, Func<Vane<T>, bool> next);

        bool Visit<T>(SourceVane<T> vane);
        bool Visit<T>(SourceVane<T> vane, Func<SourceVane<T>, bool> next);

        bool Visit<T>(FeatherVane<T> vane);
        bool Visit<T>(FeatherVane<T> vane, Func<FeatherVane<T>, bool> next);
    }
}