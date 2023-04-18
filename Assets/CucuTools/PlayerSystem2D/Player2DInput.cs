using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public abstract class Player2DInput : CucuBehaviour
    {
    }
    
    public abstract class Player2DInput<TPlayer> : Player2DInput where TPlayer : Player2DController
    {
        [SerializeField] private TPlayer _player = default;

        public TPlayer player
        {
            get => _player;
            set => _player = value;
        }
    }
}