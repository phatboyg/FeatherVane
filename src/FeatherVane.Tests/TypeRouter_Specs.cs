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
            var lambda = new Execute<ConsumeContext<A>>(context => Console.WriteLine("Body: {0}", context.Data.Body.Value));

            Vane<ConsumeContext<A>> messageAVane = VaneFactory.Connect(new Success<ConsumeContext<A>>(), lambda);

            var typeRouter = new TypeRouter<ConsumeContext>(context => context.Data.ContextType);
            typeRouter.Add(messageAVane, x =>
                {
                    ConsumeContext<A> context;
                    x.Data.TryGetContext(out context);

                    return x.CreateProxy(context);
                });

            var messageVane = VaneFactory.Connect(new Unhandled<ConsumeContext>(), typeRouter);

            var message = new MessageConsumeContext<A>(new A {Value = "Hello!"});

            messageVane.Execute(message);
        }

        interface ConsumeContext
        {
            Type MessageType { get; }

            Type ContextType { get; }

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

            public Type ContextType
            {
                get { return typeof(ConsumeContext<T>); }
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
