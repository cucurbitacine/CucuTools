namespace CucuTools.PlayerSystem
{
    public abstract class PlayerController : CucuBehaviour
    {
        
    }

    public abstract class PlayerInput : CucuBehaviour
    {
        public abstract PlayerController playerController { get; }
    }
    
    public abstract class PlayerInput<T> : PlayerInput
        where T : PlayerController
    {
        public T player;

        public sealed override PlayerController playerController => player;
    }
}