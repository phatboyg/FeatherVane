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
    using System.Threading.Tasks;
    using Taskell;


    /// <summary>
    /// Executes a Task as part of the Vane
    /// </summary>
    /// <typeparam name="T">The Vane type</typeparam>
    public class ExecuteTaskFeather<T> :
        Feather<T>
    {
        readonly Func<Payload<T>, Task> _continuationTask;

        public ExecuteTaskFeather(Func<Payload<T>, Task> continuationTask)
        {
            _continuationTask = continuationTask;
        }

        void Feather<T>.Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            composer.Execute(() => _continuationTask(payload));

            next.Compose(composer, payload);
        }
    }
}