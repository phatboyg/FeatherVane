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
    using VaneBuilders;
    using Vanes;


    public class LoggerConfiguratorImpl<T> :
        LoggerConfigurator<T>,
        VaneBuilderConfigurator<T>
    {
        Func<Payload<T>, string> _format;
        TextWriter _output;

        public LoggerConfiguratorImpl()
        {
            _format = DefaultFormat;
        }

        LoggerConfigurator<T> LoggerConfigurator<T>.SetOutput(TextWriter output)
        {
            _output = output;
            return this;
        }

        LoggerConfigurator<T> LoggerConfigurator<T>.SetFormat(Func<Payload<T>, string> format)
        {
            _format = format;
            return this;
        }

        void VaneBuilderConfigurator<T>.Configure(VaneBuilder<T> builder)
        {
            var logger = new Logger<T>(_output, _format);
            builder.Add(logger);
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            if (_format == null)
                yield return this.Failure("Format", "must specify a log message format");

            if (_output == null)
                yield return this.Failure("Output", "must specify an output text writer");
        }

        static string DefaultFormat(Payload<T> payload)
        {
            return payload.Data.ToString();
        }
    }
}