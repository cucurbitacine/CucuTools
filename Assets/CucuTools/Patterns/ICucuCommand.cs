namespace CucuTools.Patterns
{
    /// <summary>
    /// Command entity
    /// </summary>
    public interface ICucuCommand
    {
        /// <summary>
        /// Execute command
        /// </summary>
        void Execute();
        
        /// <summary>
        /// Undo command
        /// </summary>
        void Undo();
    }

    /// <inheritdoc />
    public abstract class CommandEntity : ICucuCommand
    {
        /// <inheritdoc />
        public abstract void Execute();
        
        /// <inheritdoc />
        public abstract void Undo();
    }
}