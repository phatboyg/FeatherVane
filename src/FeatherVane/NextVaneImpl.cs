namespace FeatherVane
{
    using System;

    public class NextVaneImpl<T> :
        NextVane<T>
    {
        readonly NextVane<T> _nextVane;
        readonly Vane<T> _vane;

        public NextVaneImpl(Vane<T> vane, NextVane<T> nextVane)
        {
            _vane = vane;
            _nextVane = nextVane;
        }

        public Action<T> Handle(T context)
        {
            return _vane.Handle(context, _nextVane);
        }
    }
}