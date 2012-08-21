namespace FeatherVane
{
    using System;

    public interface Vane<T>
    {
        Action<T> Handle(T context, NextVane<T> next);
    }
}