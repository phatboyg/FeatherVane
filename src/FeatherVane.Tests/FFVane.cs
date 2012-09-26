namespace FeatherVane.Tests
{
    public interface FFVane<T>
        where T : class
    {
        Agenda<T> AssignPlan(Planner<T> planner, Payload<T> payload);
    }
}