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
namespace FeatherVane.Feathers
{
    using System;
    using System.IO;
    using System.Text;
    using Taskell;


    /// <summary>
    /// An async logger writes to the Stream asynchronously to force a non-synchronous Task
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LogAsyncFeather<T> :
        Feather<T>
    {
        readonly Func<Payload<T>, string> _format;
        readonly Stream _output;
        readonly Encoding _outputEncoding;

        public LogAsyncFeather(Stream output, Func<Payload<T>, string> format, Encoding outputEncoding = null)
        {
            _output = output;
            _format = format;
            _outputEncoding = outputEncoding ?? Encoding.UTF8;
        }

        void Feather<T>.Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            composer.Execute(() =>
                {
                    string message = _format(payload);

                    byte[] data = _outputEncoding.GetBytes(message);

                    return _output.WriteAsync(data, 0, data.Length, composer.CancellationToken);
                });

            next.Compose(composer, payload);
        }
    }
}