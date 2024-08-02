namespace CucuTools.StateMachines
{
    public interface IContextHolder<TContext>
    {
        /// <summary>
        /// Context of class
        /// </summary>
        public TContext Context { get; }

        /// <summary>
        /// Set new <see cref="Context"/>
        /// </summary>
        /// <param name="context"></param>
        public void SetContext(TContext context);
    }
}