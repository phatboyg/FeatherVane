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
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using NUnit.Framework;


    [TestFixture]
    public class ActorThoughts_Specs
    {
        [Test]
        public void FirstTestName()
        {
        }


        interface Actor
        {
            void Post<T>(T message);
            // PostAndReply<T,TResponse>
        }


        class ActorMailbox
        {
            readonly ConcurrentQueue<object> _messages;
            object _currentMessage = default(object);
            bool _isStarted;
            int _messageCount;
            Action<object> _react;

            public ActorMailbox(Action<object> react)
            {
                _react = react;
                _messages = new ConcurrentQueue<object>();
            }

            void Execute(bool isFirst)
            {
                Action consumeAndLoop = () =>
                    {
                        _react(_currentMessage);
                        _currentMessage = default(object);
                        int newCount = Interlocked.Decrement(ref _messageCount);
                        if (newCount != 0)
                            Execute(false);
                    };

                if (isFirst)
                    consumeAndLoop();
                else
                {
                    bool hasMessage = _messages.TryDequeue(out _currentMessage);
                    if (hasMessage)
                        consumeAndLoop();
                    else
                    {
                        Thread.SpinWait(20);
                        Execute(false);
                    }
                }
            }

            void Receive(Action<object> callback)
            {
                _react = callback;
                _isStarted = true;
            }

            void Post(object message)
            {
                while (!_isStarted)
                    Thread.SpinWait(20);

                int newCount = Interlocked.Increment(ref _messageCount);
                if (newCount == 1)
                {
                    _currentMessage = message;
                    Execute(true);
                }
                else
                    _messages.Enqueue(message);
            }
        }
    }
}