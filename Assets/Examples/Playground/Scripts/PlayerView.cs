using CucuTools.PlayerSystem;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    public class PlayerView : PlayerInput<PlayerRigidController>
    {
        public Vector3 point;

        public Transform target;

        private void Update()
        {
            if (target != null) point = target.position;

            playerCurrent.LookAt(point);
        }
    }
}