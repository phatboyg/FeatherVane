namespace FeatherVane
{
    using System.Collections.Generic;
    using System.Linq;

    public class VanePlanner<T> : 
        Planner<T>
        where T : class
    {
        IList<AgendaItem<T>> _steps = new List<AgendaItem<T>>();

        public void Add(AgendaItem<T> agendaItem)
        {
            _steps.Add(agendaItem);

        }

        public Agenda<T> CreatePlan(Payload<T> payload)
        {
            return new AgendaImpl<T>(_steps.ToArray(), payload);
        }
    }
}