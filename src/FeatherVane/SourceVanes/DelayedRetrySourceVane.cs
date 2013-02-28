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
namespace FeatherVane.SourceVanes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;


    /// <summary>
    /// Provides a graduated retry in case the source vane throws an exception
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelayedRetrySourceVane<T> :
        SourceVane<T>
    {
        readonly SourceVane<T> _sourceVane;
        readonly IEnumerable<int> _timeouts;

        public DelayedRetrySourceVane(SourceVane<T> sourceVane)
        {
            _sourceVane = sourceVane;
            _timeouts = Timeouts;
        }

        public DelayedRetrySourceVane(SourceVane<T> sourceVane, IEnumerable<int> timeouts)
        {
            _sourceVane = sourceVane;
            _timeouts = timeouts;
        }

        static IEnumerable<int> Timeouts
        {
            get
            {
                yield return 0;
                yield return 100;
                yield return 1000;
                yield return 2000;
                yield return 5000;
                yield return 10000;
                while (true)
                    yield return 30000;
            }
        }

        public void Compose<TPayload>(Composer composer, Payload<TPayload> payload,
            Vane<Tuple<TPayload, T>> next)
        {
            IEnumerator<int> timeoutEnumerator = null;
            bool useTimeout = false;
            composer.Execute(() => timeoutEnumerator = _timeouts.GetEnumerator());

            Func<Composer, Task> nextTask = null;
            nextTask = outer => outer.ComposeTask(payload, (inner, taskPayload) =>
                {
                    if (useTimeout)
                    {
                        int retryDelay = timeoutEnumerator.Current;
                        inner.Delay(retryDelay);
                    }

                    var nextVane = new Next<TPayload>(next);

                    inner.Execute(() => inner.ComposeTask(_sourceVane, taskPayload, nextVane));

                    inner.Compensate(x =>
                        {
                            if (!nextVane.SourceCompleted)
                            {
                                useTimeout = timeoutEnumerator.MoveNext();
                                if (useTimeout)
                                    return x.Task(nextTask(inner));
                            }

                            return x.Throw();
                        });
                });

            composer.Execute(() => nextTask(composer));

            composer.Finally(() => timeoutEnumerator.Dispose());
        }


        class Next<TPayload> :
            Vane<Tuple<TPayload, T>>
        {
            readonly Vane<Tuple<TPayload, T>> _nextVane;
            bool _sourceCompleted;

            public Next(Vane<Tuple<TPayload, T>> nextVane)
            {
                _nextVane = nextVane;
            }

            public bool SourceCompleted
            {
                get { return _sourceCompleted; }
            }

            public void Compose(Composer composer, Payload<Tuple<TPayload, T>> payload)
            {
                // flag the sourceVane as completed to avoid retrying an exception throw by next
                composer.Execute(() => _sourceCompleted = true);

                _nextVane.Compose(composer, payload);
            }
        }
    }
}