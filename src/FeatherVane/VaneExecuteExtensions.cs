namespace FeatherVane
{
    using System.Diagnostics;
    using Execution;
    using Payloads;

    [DebuggerNonUserCode]
    public static class VaneExecuteExtensions
    {
        /// <summary>
        /// Handles a payload with a vane
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T>(this Vane<T> vane, T body)
        {
            var payload = new PayloadImpl<T>(body);

            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload with a vane
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T>(this Vane<T> vane, Payload<T> payload)
        {
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1>(this Vane<T> vane, T body, T1 ctx1) 
            where T1 : class
        {
            var payload = new PayloadImpl<T>(body); 
            payload.GetOrAdd(() => ctx1);
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2) 
            where T1 : class
            where T2 : class
        {
            var payload = new PayloadImpl<T>(body); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2);
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3) 
            where T1 : class
            where T2 : class
            where T3 : class
        {
            var payload = new PayloadImpl<T>(body); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3);
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            var payload = new PayloadImpl<T>(body); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4);
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4,T5>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
        {
            var payload = new PayloadImpl<T>(body); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5);
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4,T5,T6>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
        {
            var payload = new PayloadImpl<T>(body); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6);
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4,T5,T6,T7>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
        {
            var payload = new PayloadImpl<T>(body); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7);
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4,T5,T6,T7,T8>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8) 
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
        {
            var payload = new PayloadImpl<T>(body); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8);
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9) 
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
            var payload = new PayloadImpl<T>(body); 
            payload.GetOrAdd(() => ctx1); 
            payload.GetOrAdd(() => ctx2); 
            payload.GetOrAdd(() => ctx3); 
            payload.GetOrAdd(() => ctx4); 
            payload.GetOrAdd(() => ctx5); 
            payload.GetOrAdd(() => ctx6); 
            payload.GetOrAdd(() => ctx7); 
            payload.GetOrAdd(() => ctx8); 
            payload.GetOrAdd(() => ctx9);
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10) 
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
            var payload = new PayloadImpl<T>(body); 
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
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11) 
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
            var payload = new PayloadImpl<T>(body); 
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
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12) 
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
            var payload = new PayloadImpl<T>(body); 
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
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13) 
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
            var payload = new PayloadImpl<T>(body); 
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
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, T14 ctx14) 
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
            var payload = new PayloadImpl<T>(body); 
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
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, T14 ctx14, T15 ctx15) 
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
            var payload = new PayloadImpl<T>(body); 
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
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

        /// <summary>
        /// Handles a payload body with a vane, supplying additional context
        /// </summary>
        /// <typeparam name="T">The context type of the Vane</typeparam>
        /// <param name="vane">The vane itself</param>
        /// <param name="body">The body to deliver</param>
        public static bool Execute<T,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Vane<T> vane, T body, T1 ctx1, T2 ctx2, T3 ctx3, T4 ctx4, T5 ctx5, T6 ctx6, T7 ctx7, T8 ctx8, T9 ctx9, T10 ctx10, T11 ctx11, T12 ctx12, T13 ctx13, T14 ctx14, T15 ctx15, T16 ctx16) 
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
            var payload = new PayloadImpl<T>(body); 
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
            var planner = new AgendaPlanner<T>();

            Agenda<T> agenda = vane.Plan(planner, payload);

            return agenda.Execute();
        }

    }
}