namespace FeatherVane.Vanes
{
    using System;
    using System.Transactions;

    public class Transaction<T> :
        Vane<T>
    {
        TransactionScopeOption _scopeOptions;

        public Transaction()
        {
            _scopeOptions = TransactionScopeOption.Required;
        }

        public Action<T> Handle(T context, NextVane<T> next)
        {
            Action<T> nextHandler = next.Handle(context);
            if (nextHandler == null)
                return null;

            return input =>
                {
                    using (var scope = new TransactionScope(_scopeOptions))
                    {
                        nextHandler(input);

                        scope.Complete();
                    }
                };
        }
    }
}