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
    using System.Threading.Tasks;


    public class TaskCompensation :
        Compensation
    {
        static readonly Result _completed = new Result {Task = TaskUtil.Completed()};
        readonly Exception _exception;
        readonly Task _task;

        public TaskCompensation(Task task)
        {
            _task = task;
            _exception = _task.Exception.GetBaseException();
        }

        public Exception Exception
        {
            get { return _exception; }
        }

        public CompensationResult Handled()
        {
            return _completed;
        }

        public CompensationResult Task(Task task)
        {
            return new Result {Task = task};
        }

        public CompensationResult Throw(Exception ex)
        {
            return new Result {Task = TaskUtil.CompletedError<object>(ex)};
        }

        public CompensationResult Throw()
        {
            return new Result {Task = _task};
        }


        struct Result :
            CompensationResult
        {
            public Task Task { get; set; }
        }
    }
}