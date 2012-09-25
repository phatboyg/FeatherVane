namespace FeatherVane.Tests
{
    public class Logger
    {


        class LogStep<T> :
            Step<T>
            where T : class
        {
            public bool Execute(Plan<T> plan)
            {
                // do something good, and if it goes well

                if (plan.Execute())
                    return true;

                return plan.Compensate();
            }

            public bool Compensate(Plan<T> plan)
            {
                // undo our stuff

                return plan.Compensate();
            }
        }
    }
}