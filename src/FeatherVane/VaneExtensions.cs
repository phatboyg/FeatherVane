namespace FeatherVane
{
    public static class VaneExtensions
    {
        public static NextVane<T> WithNext<T>(this Vane<T> vane, NextVane<T> next)
        {
            return new NextVaneImpl<T>(vane, next);
        }
    }
}