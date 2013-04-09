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
namespace FeatherVane.Tests.Messaging
{
    using System.Threading;
    using FeatherVane.Messaging;
    using FeatherVane.Messaging.Payloads;
    using NUnit.Framework;
    using Taskell;


    [TestFixture]
    public class When_sending_a_message
    {
        [Test]
        public void Should_be_able_to_get_the_message_type()
        {
            var messageVane = new MessageFeather();
            Vane<Message> vane = VaneFactory.Success(messageVane);

            var alpha = new Alpha();
            var payload = new MessagePayload<Alpha>(alpha);

            CompositionExtensions.Compose(vane, payload, CancellationToken.None).Wait();

            Assert.IsTrue(ReferenceEquals(alpha, messageVane.Message));
        }


        class MessageFeather :
            Feather<Message>
        {
            Alpha _message;

            public Alpha Message
            {
                get { return _message; }
            }

            public void Compose(Composer composer, Payload<Message> payload, Vane<Message> next)
            {
                Message<Alpha> alphaMessage;
                if (payload.Data.TryGetAs(out alphaMessage))
                    composer.Execute(() => { _message = alphaMessage.Body; });

                next.Compose(composer, payload);
            }
        }


        class Alpha
        {
        }
    }
}