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
namespace FeatherVane.SourceVanes
{
    using System;
    using Internals.Extensions;


    /// <summary>
    /// Throwing an ObjectNotFoundException if composed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Missing<T> :
        SourceVane<T>
    {
        Func<Exception> _exceptionFactory;

        public Missing()
        {
            _exceptionFactory = CreateDefaultException;
        }

        public Missing(Func<Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory;
        }

        public void Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, T>> next)
        {
            composer.Failed(_exceptionFactory());
        }

        static ObjectNotFoundException CreateDefaultException()
        {
            return new ObjectNotFoundException(string.Format("The object was not found: {0}", typeof(T).GetTypeName()));
        }
    }
}