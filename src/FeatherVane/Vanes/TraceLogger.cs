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
    using System.Diagnostics;

    public class TraceLogger<T> :
        Vane<T>
    {
        readonly Func<T, string> _getTraceOutput;

        public TraceLogger(Func<T, string> getTraceOutput)
        {
            _getTraceOutput = getTraceOutput;
        }

        public VaneHandler<T> GetHandler(T context, NextVane<T> next)
        {
            return next.GetHandler(context).Intercept(Handler);
        }

        void Handler(T context, VaneHandler<T> nextHandler)
        {
            string output = _getTraceOutput(context);

            Trace.WriteLine(output);

            nextHandler.Handle(context);
        }
    }
}