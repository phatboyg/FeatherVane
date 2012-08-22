namespace FeatherVane.Tests
{
    using System;
    using NUnit.Framework;
    using Vanes;

    [TestFixture]
    public class Using_the_type_router_with_a_type_wrapping_context
    {
        [Test]
        public void Should_allow_the_type_to_be_dispatched()
        {
            var lambda = new LambdaAction<ConsumeContext<A>>(context => Console.WriteLine("Body: {0}", context.Body.Body.Value));

            NextVane<ConsumeContext<A>> messageAVane = NextVane.Connect(new Success<ConsumeContext<A>>(), lambda);

            var typeRouter = new TypeRouter<ConsumeContext>(context => typeof(ConsumeContext<>).MakeGenericType(context.Body.MessageType));
            typeRouter.Add(messageAVane, x =>
                {
                    ConsumeContext<A> context;
                    x.Body.TryGetContext(out context);

                    return x.CreateDelegatingVaneContext(context);
                });

            var messageVane = NextVane.Connect(new Unhandled<ConsumeContext>(), typeRouter);

            var message = new MessageConsumeContext<A>(new A {Value = "Hello!"});

            messageVane.Handle(message);
        }

        interface ConsumeContext
        {
            Type MessageType { get; }

            bool TryGetContext<T>(out ConsumeContext<T> context);
        }

        interface ConsumeContext<T> :
            ConsumeContext
        {
            T Body { get; }
            
        }

        class A
        {
            public string Value { get; set; }
           
        }

        class MessageConsumeContext<T> :
            ConsumeContext<T>
        {
            readonly T _body;

            public MessageConsumeContext(T body)
            {
                _body = body;
            }

            public Type MessageType
            {
                get { return typeof(T); }
            }

            public bool TryGetContext<T1>(out ConsumeContext<T1> context)
            {
                context = this as MessageConsumeContext<T1>;
                return context != null;
            }

            public T Body
            {
                get { return _body; }
            }
        }
    }
}
