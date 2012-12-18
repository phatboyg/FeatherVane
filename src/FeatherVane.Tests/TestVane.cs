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
namespace FeatherVane.Tests
{
    using System.Diagnostics;


    [DebuggerNonUserCode]
    public class TestVane :
        FeatherVane<TestSubject>
    {
        public bool AssignCalled { get; set; }
        public bool ExecuteCalled { get; set; }
        public bool CompensateCalled { get; set; }

        public void Compose(Composer<TestSubject> composer, Payload<TestSubject> payload, Vane<TestSubject> next)
        {
            AssignCalled = true;

            composer.Execute(() => ExecuteCalled = true);
            next.Compose(composer, payload);
            composer.Compensate(x =>
                {
                    CompensateCalled = true;
                    return x.Throw();
                });
        }
    }
}