namespace FeatherVane.Vanes
{
    using System;
    using System.Diagnostics;

    public class TraceLogger<T> :
        Vane<T>
    {
        readonly Func<T, string> _getTraceOutput;

        public TraceLogger(Func<T, string> getTraceOutput)
        {
            _getTraceOutput = getTraceOutput;
        }

        public Action<T> Handle(T context, NextVane<T> next)
        {
            Action<T> action = next.Handle(context);

            return input => Handler(input, action);
        }

        void Handler(T input, Action<T> next)
        {
            string output = _getTraceOutput(input);

            Trace.WriteLine(output);

            next(input);
        }
    }
}