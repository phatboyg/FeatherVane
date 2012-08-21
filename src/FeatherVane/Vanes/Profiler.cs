namespace FeatherVane.Vanes
{
    using System;
    using System.Diagnostics;


    public class Profiler<T> :
        Vane<T>
    {
        public Action<T> Handle(T context, NextVane<T> next)
        {
            Guid timingId = Guid.NewGuid(); // change to use NewId
            DateTime startTime = DateTime.UtcNow;
            Stopwatch stopwatch = Stopwatch.StartNew();

            Action<T> nextHandler = next.Handle(context);

            return input =>
                {
                    try
                    {
                        nextHandler(input);
                    }
                    finally
                    {
                        stopwatch.Stop();

                        Trace.WriteLine(timingId.ToString("N") + ": "
                                        + startTime.ToString("yyyyMMdd HHmmss.fff") + " "
                                        + stopwatch.ElapsedMilliseconds + "ms");
                    }
                };
        }
    }
}