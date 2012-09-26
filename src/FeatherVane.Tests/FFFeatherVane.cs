namespace FeatherVane.Tests
{
    public interface FFFeatherVane<T>
        where T : class
    {
        Agenda<T> AssignPlan(Planner<T> planner, Payload<T> payload, FFVane<T> next);
    }
}