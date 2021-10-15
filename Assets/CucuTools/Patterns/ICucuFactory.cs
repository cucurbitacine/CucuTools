namespace CucuTools.Patterns
{
    /// <summary>
    /// Factory of objects of type <see cref="TObject"/>
    /// </summary>
    /// <typeparam name="TObject">Objects type family</typeparam>
    public interface ICucuFactory<in TObject>
    {
        /// <summary>
        /// Create object of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T">Objects type</typeparam>
        /// <returns></returns>
        T Create<T>() where T : TObject;
    }

    /// <inheritdoc />
    public abstract class FactoryEntity<TClass> : ICucuFactory<TClass> where TClass : class
    {
        /// <inheritdoc />
        public abstract T Create<T>() where T : TClass;
    }
}