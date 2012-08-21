namespace FeatherVane.Vanes
{
    using System;

    public class EndVane<T> :
        Vane<T>,
        NextVane<T>
    {
        public Action<T> Handle(T context, NextVane<T> next)
        {
            return input => { };
        }

        public Action<T> Handle(T context)
        {
            return input => { };
        }
    }
}