namespace FeatherVane.RabbitMQIntegration.ConnectionManagement
{
    /// <summary>
    /// An factory for a pooled object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface PooledObjectFactory<T>
        where T : class
    {
        /// <summary>
        /// Create a new instance of the pooled type
        /// </summary>
        /// <returns>A new instance of the pooled type</returns>
        T New();

        /// <summary>
        /// Disposes of a pooled type instance, calling IDisposable.Dispose if necessary
        /// </summary>
        /// <param name="instance"></param>
        void Dispose(T instance);

        /// <summary>
        /// Activates an instance of an object, if it was previously deactivated
        /// </summary>
        /// <param name="instance"></param>
        void Activate(T instance);

        /// <summary>
        /// Passivates an instance of an object, putting it into a sleeping or inactive state
        /// </summary>
        /// <param name="instance"></param>
        void Passivate(T instance);

        /// <summary>
        /// Validate that an object is valid and usable
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        bool Validate(T instance);
    }
}