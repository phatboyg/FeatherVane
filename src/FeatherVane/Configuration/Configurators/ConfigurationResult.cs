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
namespace FeatherVane.Configurators
{
    using System.Collections.Generic;


    public interface ConfigurationResult
    {
        /// <summary>
        /// True if at least one configurator failed
        /// </summary>
        bool ContainsFailure { get; }

        /// <summary>
        /// The configuration message (combined result of all messages)
        /// </summary>
        string Message { get; }

        /// <summary>
        /// The results from the configuration (both success and failure)
        /// </summary>
        IEnumerable<ValidateResult> Results { get; }
    }
}