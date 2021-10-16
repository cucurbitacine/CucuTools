namespace CucuTools.Interactables
{
    /// <inheritdoc cref="CucuTools.Interactables.ICucuInteractable" />
    public abstract class CucuInteractable : CucuBehaviour, ICucuInteractable
    {
        /// <inheritdoc />
        public abstract bool IsEnabled { get; set; }
        /// <inheritdoc />
        public abstract InteractInfo InteractInfo { get; }
        /// <inheritdoc />
        public abstract void Idle();
        /// <inheritdoc />
        public abstract void Hover(ICucuContext context);
        /// <inheritdoc />
        public abstract void Press(ICucuContext context);
    }
}