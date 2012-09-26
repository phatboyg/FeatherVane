namespace FeatherVane.Tests
{
    public interface VaneCommand<T>
        where T : class
    {
        bool Execute(AgendaItem<T> agendaItem);
    }
}