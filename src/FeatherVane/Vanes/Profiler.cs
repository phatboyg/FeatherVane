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

    public class Profiler<T> :
        Vane<T>
    {
        readonly ProfilerSettings _settings;

        public Profiler(TimeSpan trivialThreshold)
        {
            _settings = new ProfilerSettings(trivialThreshold);
        }

        public VaneHandler<T> GetHandler(T context, NextVane<T> next)
        {
            return new ProfilerVaneHandler(_settings, context, next);
        }

        class ProfilerSettings
        {
            public ProfilerSettings(TimeSpan trivialThreshold)
            {
                TrivialThreshold = trivialThreshold;
            }

            public TimeSpan TrivialThreshold { get; private set; }
        }

        class ProfilerVaneHandler :
            VaneHandler<T>
        {
            readonly VaneHandler<T> _handler;
            readonly ProfilerSettings _settings;
            readonly DateTime _startTime;
            readonly Stopwatch _stopwatch;
            readonly Guid _timingId;

            public ProfilerVaneHandler(ProfilerSettings settings, T context, NextVane<T> next)
            {
                _settings = settings;
                _timingId = Guid.NewGuid();
                _startTime = DateTime.UtcNow;
                _stopwatch = Stopwatch.StartNew();

                _handler = next.GetHandler(context);
            }

            public void Handle(T context)
            {
                try
                {
                    _handler.Handle(context);
                }
                finally
                {
                    _stopwatch.Stop();

                    if (_stopwatch.Elapsed > _settings.TrivialThreshold)
                    {
                        Trace.WriteLine(_timingId.ToString("N") + ": "
                                        + _startTime.ToString("yyyyMMdd HHmmss.fff") + " "
                                        + _stopwatch.ElapsedMilliseconds + "ms");
                    }
                }
            }
        }
    }
}