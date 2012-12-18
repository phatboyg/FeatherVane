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
namespace FeatherVane.Vanes
{
    using System;
    using System.IO;
    using System.Text;


    /// <summary>
    /// An async logger writes to the Stream asynchronously to force a non-synchronous Task
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncLogger<T> :
        FeatherVane<T>
    {
        readonly Func<Payload<T>, string> _getLogMessage;
        readonly Stream _output;
        readonly Encoding _outputEncoding;

        public AsyncLogger(Stream output, Func<Payload<T>, string> getLogMessage, Encoding outputEncoding = null)
        {
            _output = output;
            _getLogMessage = getLogMessage;
            _outputEncoding = outputEncoding ?? Encoding.UTF8;
        }

        void FeatherVane<T>.Build(Builder<T> builder, Payload<T> payload, Vane<T> next)
        {
            builder.Execute(() =>
                {
                    string message = _getLogMessage(payload);

                    byte[] data = _outputEncoding.GetBytes(message);

                    return _output.WriteAsync(data, 0, data.Length, builder.CancellationToken);
                });

            next.Build(builder, payload);
        }
    }
}