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
namespace FeatherVane.FeatherVaneConfigurators
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Configurators;
    using Feathers;
    using VaneBuilders;


    public class ProfilerConfiguratorImpl<T> :
        ProfilerConfigurator<T>,
        VaneBuilderConfigurator<T>
    {
        TextWriter _output;
        TimeSpan _threshold;

        ProfilerConfigurator<T> ProfilerConfigurator<T>.SetOutput(TextWriter output)
        {
            _output = output;

            return this;
        }

        public ProfilerConfigurator<T> Threshold(TimeSpan minimumDuration)
        {
            _threshold = minimumDuration;

            return this;
        }

        void VaneBuilderConfigurator<T>.Configure(VaneBuilder<T> builder)
        {
            var profiler = new ProfilerFeather<T>(_output, _threshold);
            builder.Add(profiler);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            if (_output == null)
                yield return this.Failure("Output", "must specify an output text writer");

            if (_threshold < TimeSpan.Zero)
                yield return this.Failure("Threshold", "must be >= 0");
        }
    }
}