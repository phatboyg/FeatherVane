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
namespace FeatherVane
{
    using System;
    using System.Runtime.Serialization;
    using Internals.Extensions;

    [Serializable]
    public class UnhandledException :
        FeatherVaneException
    {
        public UnhandledException()
        {
        }

        public UnhandledException(string message)
            : base(message)
        {
        }

        public UnhandledException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UnhandledException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public static UnhandledException New<T>(Payload<T> payload) 
            where T : class
        {
            return new UnhandledException("The context was not handled: " + typeof(T).GetTypeName());
        }
    }
}