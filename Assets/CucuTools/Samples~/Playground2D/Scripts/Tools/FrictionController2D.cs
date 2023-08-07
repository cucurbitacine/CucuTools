using CucuTools.PlayerSystem2D;
using UnityEngine;

namespace Samples.Playground2D.Scripts.Tools
{
    public class FrictionController2D : ToolController2D
    {
        [Header("Friction")]
        public bool overrideFriction = false;
        public float idleFriction = 100f;
        
        private void Update()
        {
            player2d.idleMat.friction = overrideFriction ? idleFriction : PlayerController2D.IdleFrictionDefault;
        }
    }
}