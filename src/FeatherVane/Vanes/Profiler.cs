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
    using System.IO;

    public class Profiler<T> :
        FeatherVane<T>
        where T : class
    {
        readonly ProfilerSettings _settings;

        public Profiler(TextWriter writer, TimeSpan trivialThreshold)
        {
            _settings = new ProfilerSettings(writer, trivialThreshold);
        }

        public Plan<T> AssignPlan(Planner<T> planner, Payload<T> payload, Vane<T> next)
        {
            var step = new ProfilerStep(_settings);

            planner.Add(step);

            return next.AssignPlan(planner, payload);
        }

        class ProfilerSettings
        {
            public ProfilerSettings(TextWriter writer, TimeSpan trivialThreshold)
            {
                Writer = writer;
                TrivialThreshold = trivialThreshold;
            }

            public TimeSpan TrivialThreshold { get; private set; }

            public TextWriter Writer { get; private set; }
        }

        class ProfilerStep :
            Step<T>
        {
            readonly ProfilerSettings _settings;
            readonly DateTime _startTime;
            readonly Stopwatch _stopwatch;
            readonly Guid _timingId;

            public ProfilerStep(ProfilerSettings settings)
            {
                _settings = settings;
                _timingId = Guid.NewGuid();
                _startTime = DateTime.UtcNow;
                _stopwatch = Stopwatch.StartNew();
            }

            public bool Execute(Plan<T> plan)
            {
                try
                {
                    return plan.Execute();
                }
                finally
                {
                    _stopwatch.Stop();

                    if (_stopwatch.Elapsed > _settings.TrivialThreshold)
                    {
                        _settings.Writer.WriteLine(_timingId.ToString("N") + ": "
                                                   + _startTime.ToString("yyyyMMdd HHmmss.fff") + " "
                                                   + _stopwatch.ElapsedMilliseconds + "ms");
                    }
                }
            }

            public bool Compensate(Plan<T> plan)
            {
                return plan.Compensate();
            }
        }
    }
}