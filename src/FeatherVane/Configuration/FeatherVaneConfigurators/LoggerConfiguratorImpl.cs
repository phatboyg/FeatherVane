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
namespace FeatherVane.FeatherVaneConfigurators
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Configurators;
    using FeatherVaneBuilders;
    using VaneBuilders;


    public class LoggerConfiguratorImpl<T> :
        LoggerConfigurator<T>,
        VaneBuilderConfigurator<T>
    {
        Func<Payload<T>, string> _formatter;
        TextWriter _output;

        public LoggerConfiguratorImpl()
        {
            _formatter = DefaultFormatter;
        }

        LoggerConfigurator<T> LoggerConfigurator<T>.SetOutput(TextWriter output)
        {
            _output = output;

            return this;
        }

        LoggerConfigurator<T> LoggerConfigurator<T>.SetFormatter(Func<Payload<T>, string> formatter)
        {
            _formatter = formatter;

            return this;
        }

        VaneBuilder<T> VaneBuilderConfigurator<T>.Configure(VaneBuilder<T> builder)
        {
            var featherVaneBuilder = new LoggerBuilder<T>(_output, _formatter);

            builder.Add(featherVaneBuilder);

            return builder;
        }

        IEnumerable<ValidateResult> Configurator.Validate()
        {
            if (_formatter == null)
                yield return this.Failure("GetLogMessage", "must specify a log message formatter");

            if (_output == null)
                yield return this.Failure("Output", "must specify an output text writer");
        }

        static string DefaultFormatter(Payload<T> payload)
        {
            return payload.Data.ToString();
        }
    }
}