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
    using System.Diagnostics;
    using System.IO;


    public class ProfilerFeather<T> :
        Feather<T>
    {
        readonly ProfilerSettings _settings;

        public ProfilerFeather(TextWriter writer, TimeSpan trivialThreshold)
        {
            _settings = new ProfilerSettings(writer, trivialThreshold);
        }

        public void Compose(Composer composer, Payload<T> payload, Vane<T> next)
        {
            ProfilerSettings settings = _settings;

            ProfilerInstance instance = null;
            composer.Execute(() => instance = new ProfilerInstance(settings));

            next.Compose(composer, payload);

            composer.Finally(() =>
                {
                    if (instance != null)
                        instance.Complete();
                });
        }


        class ProfilerInstance
        {
            readonly ProfilerSettings _settings;
            readonly DateTime _startTime;
            readonly Stopwatch _stopwatch;
            readonly Guid _timingId;

            internal ProfilerInstance(ProfilerSettings settings)
            {
                _settings = settings;
                _timingId = Guid.NewGuid();
                _startTime = DateTime.UtcNow;
                _stopwatch = Stopwatch.StartNew();
            }

            public void Complete()
            {
                _stopwatch.Stop();

                if (_stopwatch.Elapsed > _settings.TrivialThreshold)
                {
                    _settings.Writer.WriteLine(_timingId.ToString("N") + ": "
                                               + _startTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + " "
                                               + _stopwatch.ElapsedMilliseconds + "ms");
                }
            }
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
    }
}