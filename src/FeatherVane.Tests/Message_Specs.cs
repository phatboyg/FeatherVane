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
    using System.Threading;
    using FeatherVane.Actors;
    using NUnit.Framework;


    [TestFixture]
    public class When_sending_a_message
    {
        [Test]
        public void Should_be_able_to_get_the_message_type()
        {
            var messageVane = new MessageVane();
            Vane<Message> vane = VaneBuilder.Success(messageVane);

            var alpha = new Alpha();
            var payload = new MessagePayload<Alpha>(alpha);

            TaskBuilder.Build(vane, payload, CancellationToken.None).Wait();

            Assert.IsTrue(ReferenceEquals(alpha, messageVane.Message));
        }


        class MessageVane :
            FeatherVane<Message>
        {
            Alpha _message;

            public Alpha Message
            {
                get { return _message; }
            }

            public void Build(Builder<Message> builder, Payload<Message> payload, Vane<Message> next)
            {
                Message<Alpha> alphaMessage;
                if (payload.Data.TryGetAs(out alphaMessage))
                    builder.Execute(() => { _message = alphaMessage.Body; });

                next.Build(builder, payload);
            }
        }


        class Alpha
        {
        }
    }
}