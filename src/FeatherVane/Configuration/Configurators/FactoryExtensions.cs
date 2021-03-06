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
    using Factories;


    public static class FactoryExtensions
    {
        public static Vane<T> ValidateAndCreate<T>(this VaneFactory<T> vaneFactory)
        {
            var result = new ValidateConfigurationResult(vaneFactory.Validate());
            if (result.ContainsFailure)
                throw new VaneConfigurationException(result);

            return vaneFactory.Create();
        }

        public static Vane<T> Create<T>(this VaneFactory<T> vaneFactory)
        {
            return vaneFactory.Create();
        }

        public static IEnumerable<ValidateResult> Validate<T>(this VaneFactory<T> vaneFactory)
        {
            return vaneFactory.Validate();
        }

        public static SourceVane<T> ValidateAndCreate<T>(this SourceVaneFactory<T> vaneFactory)
        {
            var result = new ValidateConfigurationResult(vaneFactory.Validate());
            if (result.ContainsFailure)
                throw new VaneConfigurationException(result);

            return vaneFactory.Create();
        }

        public static SourceVane<T> Create<T>(this SourceVaneFactory<T> vaneFactory)
        {
            return vaneFactory.Create();
        }

        public static IEnumerable<ValidateResult> Validate<T>(this SourceVaneFactory<T> vaneFactory)
        {
            return vaneFactory.Validate();
        }
    }
}