using UnityEngine;

namespace CucuTools.PlayerSystem
{
    public abstract class PlayerInput : CucuBehaviour
    {
        public abstract PlayerController player { get; }
    }
    
    public abstract class PlayerInput<TPlayer> : PlayerInput where TPlayer : PlayerController
    {
        [SerializeField] private TPlayer _player = default;

        public TPlayer playerTyped
        {
            get => _player;
            set => _player = value;
        }

        public sealed override PlayerController player => _player;
    }
}