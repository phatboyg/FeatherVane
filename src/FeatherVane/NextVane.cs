namespace FeatherVane
{
    using System;

    public interface NextVane<T>
    {
        Action<T> Handle(T context);
    }
}