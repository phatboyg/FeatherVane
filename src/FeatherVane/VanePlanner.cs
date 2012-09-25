namespace FeatherVane
{
    using System.Collections.Generic;
    using System.Linq;

    public class VanePlanner<T> : 
        Planner<T>
        where T : class
    {
        IList<Step<T>> _steps = new List<Step<T>>();

        public void Add(Step<T> step)
        {
            _steps.Add(step);

        }

        public Plan<T> CreatePlan(Payload<T> payload)
        {
            return new VanePlan<T>(_steps.ToArray(), payload);
        }
    }
}