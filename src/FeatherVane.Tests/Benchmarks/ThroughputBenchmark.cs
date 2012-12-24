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
namespace FeatherVane.Tests.Benchmarks
{
    using System.Collections.Generic;
    using Benchmarque;

    public class ThroughputBenchmark :
        Benchmark<Throughput>
    {
        public void WarmUp(Throughput instance)
        {
            instance.Execute(new Subject());
        }

        public void Shutdown(Throughput instance)
        {
        }

        public void Run(Throughput instance, int iterationCount)
        {
            var subject = new Subject();

            for (int i = 0; i < iterationCount; i++)
            {
                instance.Execute(subject);
            }
        }

        public IEnumerable<int> Iterations
        {
            get { return new[] {1000, 100000}; }
        }
    }
}