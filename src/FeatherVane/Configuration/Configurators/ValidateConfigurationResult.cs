﻿// Copyright 2012-2012 Chris Patterson
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;


    [Serializable, DebuggerDisplay("{Message}")]
    public class ValidateConfigurationResult :
        ConfigurationResult
    {
        readonly IList<ValidateResult> _results;

        public ValidateConfigurationResult(IEnumerable<ValidateResult> results)
        {
            _results = results.ToList();
        }

        public bool ContainsFailure
        {
            get { return _results.Any(x => x.Disposition == ValidationResultDisposition.Failure); }
        }

        public IEnumerable<ValidateResult> Results
        {
            get { return _results; }
        }

        public string Message
        {
            get
            {
                string debuggerString = string.Join(", ", _results);

                return string.IsNullOrWhiteSpace(debuggerString)
                           ? ""
                           : debuggerString;
            }
        }
    }
}