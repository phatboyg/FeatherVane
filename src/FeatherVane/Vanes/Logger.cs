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
    using System;
    using System.IO;

    /// <summary>
    /// Logs to a TextWriter before the passing control to the next vane. The payload is 
    /// used to get the output text via the provider function.
    /// </summary>
    /// <typeparam name="T">The Vane type</typeparam>
    public class Logger<T> :
        FeatherVane<T>
    {
        readonly Func<Payload<T>, string> _getLogMessage;
        readonly TextWriter _output;

        /// <summary>
        /// Constructs a Logger
        /// </summary>
        /// <param name="output">The output writer for log messages</param>
        /// <param name="getLogMessage">The log message factory method</param>
        public Logger(TextWriter output, Func<Payload<T>, string> getLogMessage)
        {
            _output = output;
            _getLogMessage = getLogMessage;
        }

        public void Build(Builder<T> builder, Payload<T> payload, Vane<T> next)
        {
            builder.Execute(() =>
                {
                    string message = _getLogMessage(payload);

                    _output.WriteLine(message);
                });

            next.Build(builder, payload);
        }
    }
}