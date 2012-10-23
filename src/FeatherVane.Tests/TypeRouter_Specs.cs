namespace FeatherVane.Tests
{
    using System;
    using System.Diagnostics;
#if !NETFX_CORE
    using NUnit.Framework;
#else
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif
    using Vanes;

#if !NETFX_CORE
    [TestFixture]
#else
    [TestClass]
#endif
    public class Using_the_type_router_with_a_type_wrapping_context
    {
#if !NETFX_CORE
        [Test]
#else
         [TestMethod]
#endif
        public void Should_allow_the_type_to_be_dispatched()
        {
#if !NETFX_CORE
            var lambda = new ExecuteAction<ConsumeContext<A>>(context => Console.WriteLine("Body: {0}", context.Data.Body.Value));
#else
            var lambda = new ExecuteAction<ConsumeContext<A>>(context => Debug.WriteLine("Body: {0}", context.Data.Body.Value));
#endif
            Vane<ConsumeContext<A>> messageAVane = Vane.Connect(new Success<ConsumeContext<A>>(), lambda);

            var typeRouter = new TypeRouter<ConsumeContext>(context => context.Data.ContextType);
            typeRouter.Add(messageAVane, x =>
                {
                    ConsumeContext<A> context;
                    x.Data.TryGetContext(out context);

                    return x.CreateDelegatingPayload(context);
                });

            var messageVane = Vane.Connect(new Unhandled<ConsumeContext>(), typeRouter);

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
