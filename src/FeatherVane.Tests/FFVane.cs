namespace FeatherVane.Tests
{
    public interface FFVane<T>
        where T : class
    {
        Plan<T> AssignPlan(Planner<T> planner, Payload<T> payload);
    }
}