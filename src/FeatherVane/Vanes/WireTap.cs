namespace FeatherVane.Vanes
{
    using System;

    /// <summary>
    /// A WireTap passes the context to another Vane so that it can be observed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WireTap<T> :
        Vane<T>
    {
        readonly NextVane<T> _tap;

        public WireTap(NextVane<T> tap)
        {
            _tap = tap;
        }

        public Action<T> Handle(T context, NextVane<T> next)
        {
            _tap.Handle(context);

            return next.Handle(context);
        }
    }
}