namespace CucuTools.PlayerSystem
{
    public abstract class PlayerInput : CucuBehaviour
    {
        public abstract PlayerController player { get; }
    }

    public abstract class PlayerInput<TPlayer> : PlayerInput where TPlayer : PlayerController
    {
        public TPlayer playerCurrent;

        public sealed override PlayerController player => playerCurrent;
    }
}