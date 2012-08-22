namespace FeatherVane
{
    public static class NextVaneHandleExtensions
    {
        /// <summary>
        /// Handles a context body with a vane
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T>(this NextVane<T> vane, T body)
            where T : class
        {
            var context = new VaneContextImpl<T>(body);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1>(this NextVane<T> vane, T body, T1 ctx1) 
            where T : class
            where T1 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2) 
            where T : class
            where T1 : class
            where T2 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4,T5>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4); 
            context.GetContext(() => ctx5);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4,T5,T6>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4); 
            context.GetContext(() => ctx5); 
            context.GetContext(() => ctx6);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4,T5,T6,T7>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4); 
            context.GetContext(() => ctx5); 
            context.GetContext(() => ctx6); 
            context.GetContext(() => ctx7);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4,T5,T6,T7,T8>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4); 
            context.GetContext(() => ctx5); 
            context.GetContext(() => ctx6); 
            context.GetContext(() => ctx7); 
            context.GetContext(() => ctx8);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4); 
            context.GetContext(() => ctx5); 
            context.GetContext(() => ctx6); 
            context.GetContext(() => ctx7); 
            context.GetContext(() => ctx8); 
            context.GetContext(() => ctx9);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4); 
            context.GetContext(() => ctx5); 
            context.GetContext(() => ctx6); 
            context.GetContext(() => ctx7); 
            context.GetContext(() => ctx8); 
            context.GetContext(() => ctx9); 
            context.GetContext(() => ctx10);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
            where T11 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4); 
            context.GetContext(() => ctx5); 
            context.GetContext(() => ctx6); 
            context.GetContext(() => ctx7); 
            context.GetContext(() => ctx8); 
            context.GetContext(() => ctx9); 
            context.GetContext(() => ctx10); 
            context.GetContext(() => ctx11);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
            where T11 : class
            where T12 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4); 
            context.GetContext(() => ctx5); 
            context.GetContext(() => ctx6); 
            context.GetContext(() => ctx7); 
            context.GetContext(() => ctx8); 
            context.GetContext(() => ctx9); 
            context.GetContext(() => ctx10); 
            context.GetContext(() => ctx11); 
            context.GetContext(() => ctx12);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
            where T11 : class
            where T12 : class
            where T13 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4); 
            context.GetContext(() => ctx5); 
            context.GetContext(() => ctx6); 
            context.GetContext(() => ctx7); 
            context.GetContext(() => ctx8); 
            context.GetContext(() => ctx9); 
            context.GetContext(() => ctx10); 
            context.GetContext(() => ctx11); 
            context.GetContext(() => ctx12); 
            context.GetContext(() => ctx13);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, T14 ctx14) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
            where T11 : class
            where T12 : class
            where T13 : class
            where T14 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4); 
            context.GetContext(() => ctx5); 
            context.GetContext(() => ctx6); 
            context.GetContext(() => ctx7); 
            context.GetContext(() => ctx8); 
            context.GetContext(() => ctx9); 
            context.GetContext(() => ctx10); 
            context.GetContext(() => ctx11); 
            context.GetContext(() => ctx12); 
            context.GetContext(() => ctx13); 
            context.GetContext(() => ctx14);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, T14 ctx14, T15 ctx15) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
            where T11 : class
            where T12 : class
            where T13 : class
            where T14 : class
            where T15 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4); 
            context.GetContext(() => ctx5); 
            context.GetContext(() => ctx6); 
            context.GetContext(() => ctx7); 
            context.GetContext(() => ctx8); 
            context.GetContext(() => ctx9); 
            context.GetContext(() => ctx10); 
            context.GetContext(() => ctx11); 
            context.GetContext(() => ctx12); 
            context.GetContext(() => ctx13); 
            context.GetContext(() => ctx14); 
            context.GetContext(() => ctx15);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

        /// <summary>
        /// Handles a context body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static void Handle<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this NextVane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, T14 ctx14, T15 ctx15, T16 ctx16) 
            where T : class
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
            where T11 : class
            where T12 : class
            where T13 : class
            where T14 : class
            where T15 : class
            where T16 : class
        {
            var context = new VaneContextImpl<T>(body); 
            context.GetContext(() => ctx1); 
            context.GetContext(() => ctx2); 
            context.GetContext(() => ctx3); 
            context.GetContext(() => ctx4); 
            context.GetContext(() => ctx5); 
            context.GetContext(() => ctx6); 
            context.GetContext(() => ctx7); 
            context.GetContext(() => ctx8); 
            context.GetContext(() => ctx9); 
            context.GetContext(() => ctx10); 
            context.GetContext(() => ctx11); 
            context.GetContext(() => ctx12); 
            context.GetContext(() => ctx13); 
            context.GetContext(() => ctx14); 
            context.GetContext(() => ctx15); 
            context.GetContext(() => ctx16);

            VaneHandler<T> handler = vane.GetHandler(context);

            handler.Handle(context);
        }

    }
}