namespace FeatherVane.Tests
{
    public interface FFFeatherVane<T>
        where T : class
    {
        Plan<T> AssignPlan(Planner<T> planner, Payload<T> payload, FFVane<T> next);
    }
}