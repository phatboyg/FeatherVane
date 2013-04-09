namespace FeatherVane
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Payloads;

    [DebuggerNonUserCode]
    public static class VaneExecuteExtensions
    {
        /// <summary>
        /// Handles a payload with a vane
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The body to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T>(this Vane<T> vane, T data, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true)
        {
            var payload = new PayloadImpl<T>(data);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Executes a payload asynchronously without waiting
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The body to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T>(this Vane<T> vane, T data, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true)
        {
           var payload = new PayloadImpl<T>(data);

           return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }

        /// <summary>
        /// Handles a payload with a vane
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="payload">The payload to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T>(this Vane<T> vane, Payload<T> payload, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true)
        {
            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }  
        
        /// <summary>
        /// Handles a payload with a vane
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="payload">The payload to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T>(this Vane<T> vane, Payload<T> payload, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true)
        {
            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1>(this Vane<T> vane, T data, T1 ctx1, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1>(this Vane<T> vane, T data, T1 ctx1,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
            where T3 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
            where T3 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4,T5>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4,T5>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4,T5,T6>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4,T5,T6>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4,T5,T6,T7>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4,T5,T6,T7>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4,T5,T6,T7,T8>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4,T5,T6,T7,T8>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
        {
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10); 
            payload.GetOrAdd(() => ctx11);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10); 
            payload.GetOrAdd(() => ctx11);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10); 
            payload.GetOrAdd(() => ctx11); 
            payload.GetOrAdd(() => ctx12);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10); 
            payload.GetOrAdd(() => ctx11); 
            payload.GetOrAdd(() => ctx12);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10); 
            payload.GetOrAdd(() => ctx11); 
            payload.GetOrAdd(() => ctx12); 
            payload.GetOrAdd(() => ctx13);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10); 
            payload.GetOrAdd(() => ctx11); 
            payload.GetOrAdd(() => ctx12); 
            payload.GetOrAdd(() => ctx13);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, T14 ctx14, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10); 
            payload.GetOrAdd(() => ctx11); 
            payload.GetOrAdd(() => ctx12); 
            payload.GetOrAdd(() => ctx13); 
            payload.GetOrAdd(() => ctx14);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, T14 ctx14,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10); 
            payload.GetOrAdd(() => ctx11); 
            payload.GetOrAdd(() => ctx12); 
            payload.GetOrAdd(() => ctx13); 
            payload.GetOrAdd(() => ctx14);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, T14 ctx14, T15 ctx15, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10); 
            payload.GetOrAdd(() => ctx11); 
            payload.GetOrAdd(() => ctx12); 
            payload.GetOrAdd(() => ctx13); 
            payload.GetOrAdd(() => ctx14); 
            payload.GetOrAdd(() => ctx15);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, T14 ctx14, T15 ctx15,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10); 
            payload.GetOrAdd(() => ctx11); 
            payload.GetOrAdd(() => ctx12); 
            payload.GetOrAdd(() => ctx13); 
            payload.GetOrAdd(() => ctx14); 
            payload.GetOrAdd(() => ctx15);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static void Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, T14 ctx14, T15 ctx15, T16 ctx16, 
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10); 
            payload.GetOrAdd(() => ctx11); 
            payload.GetOrAdd(() => ctx12); 
            payload.GetOrAdd(() => ctx13); 
            payload.GetOrAdd(() => ctx14); 
            payload.GetOrAdd(() => ctx15); 
            payload.GetOrAdd(() => ctx16);

            CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously).Wait();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="data">The data to deliver</param>
        /// <param name="cancellationToken">The cancellation token to cancel</param>
        /// <param name="runSynchronously">Run synchronously if possible, otherwise force a Task</param>
        public static Task ExecuteAsync<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Vane<T> vane, T data, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, T14 ctx14, T15 ctx15, T16 ctx16,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously = true) 
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
            var payload = new PayloadImpl<T>(data); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9); 
            payload.GetOrAdd(() => ctx10); 
            payload.GetOrAdd(() => ctx11); 
            payload.GetOrAdd(() => ctx12); 
            payload.GetOrAdd(() => ctx13); 
            payload.GetOrAdd(() => ctx14); 
            payload.GetOrAdd(() => ctx15); 
            payload.GetOrAdd(() => ctx16);

            return CompositionExtensions.Compose(vane, payload, cancellationToken, runSynchronously);
        }
    }
}