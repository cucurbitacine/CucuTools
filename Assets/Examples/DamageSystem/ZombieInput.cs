using System;
using CucuTools.PlayerSystem;

namespace Examples.DamageSystem
{
    public class ZombieInput : PlayerInput<PlayerRigidController>
    {
        public PlayerRigidController target;
        
        private void Update()
        {
            player.Move(target.position - player.position);
            player.LookAt(target.eyes.position);
        }
    }
}