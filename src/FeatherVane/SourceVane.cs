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
namespace FeatherVane
{
    using System;


    /// <summary>
    /// A source vane is used to splice another type into a vane
    /// </summary>
    /// <typeparam name="T">The source type</typeparam>
    public interface SourceVane<T>
    {
        void Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, T>> next);
    }


    /// <summary>
    /// A closed source vane is used to splice a specified type into a vane
    /// </summary>
    /// <typeparam name="TInput">The vane type</typeparam>
    /// <typeparam name="T">The source type to splice</typeparam>
    public interface SourceVane<in TInput, T>
    {
        void Compose<TPayload>(Composer composer, Payload<TPayload> payload, Vane<Tuple<TPayload, T>> next)
            where TPayload : TInput;
    }
}