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

#if !NETFX_CORE
    [Serializable]
#endif
    public class ContextFactoryException :
        FeatherVaneException
    {
        public ContextFactoryException()
        {
        }

        public ContextFactoryException(string message)
            : base(message)
        {
        }

        public ContextFactoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if !NETFX_CORE
        protected ContextFactoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}