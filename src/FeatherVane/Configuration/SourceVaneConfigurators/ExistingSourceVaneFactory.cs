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
namespace FeatherVane.SourceVaneConfigurators
{
    using System.Collections.Generic;
    using Configurators;


    public class ExistingSourceVaneFactory<T> :
        SourceVaneFactory<T>
    {
        readonly SourceVane<T> _sourceVane;

        public ExistingSourceVaneFactory(SourceVane<T> sourceVane)
        {
            _sourceVane = sourceVane;
        }

        public SourceVane<T> Create()
        {
            return _sourceVane;
        }

        public IEnumerable<ValidateResult> Validate()
        {
            if (_sourceVane == null)
                yield return this.Failure("SourceVane", "must not be null");
        }
    }
}