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

    public class Logger<T> :
        Vane<T>
        where T : class
    {
        readonly Func<VaneContext<T>, string> _getLogMessage;
        readonly TextWriter _output;

        public Logger(TextWriter output, Func<VaneContext<T>, string> getLogMessage)
        {
            _output = output;
            _getLogMessage = getLogMessage;
        }

        public VaneHandler<T> GetHandler(VaneContext<T> context, NextVane<T> next)
        {
            VaneHandler<T> nextHandler = next.GetHandler(context);

            return nextHandler.InterceptWith(TraceHandler);
        }

        void TraceHandler(VaneContext<T> context, VaneHandler<T> nextHandler)
        {
            string message = _getLogMessage(context);
            _output.WriteLine(message);

            nextHandler.Handle(context);
        }
    }
}