namespace FeatherVane.Tests
{
    public interface VaneCommand<T>
        where T : class
    {
        bool Execute(Step<T> step);
    }
}