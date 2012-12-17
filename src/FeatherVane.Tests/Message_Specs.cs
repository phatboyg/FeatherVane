﻿// Copyright 2012-2012 Chris Patterson
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
    using FeatherVane.Actors;
    using NUnit.Framework;
    using Vanes;


    [TestFixture]
    public class When_sending_a_message
    {
        [Test]
        public void Should_be_able_to_get_the_message_type()
        {
            Vane<Message> vane = Vane.Connect(new Success<Message>(),
                new MessageVane());

            var payload = new MessagePayload<Alpha>(new Alpha());

            var planner = new TaskBuilder<Message>();

            vane.Build(planner, payload);
        }


        class MessageVane :
            FeatherVane<Message>
        {
            public void Build(Builder<Message> builder, Payload<Message> payload, Vane<Message> next)
            {
                Message<Alpha> alphaMessage;
                if (payload.Data.TryGet(out alphaMessage))
                    builder.Execute(() => { });

                next.Build(builder, payload);
            }
        }


        class Alpha
        {
        }
    }
}