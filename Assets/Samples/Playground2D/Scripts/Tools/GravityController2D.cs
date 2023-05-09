using CucuTools.Attributes;
using CucuTools.PlayerSystem2D;
using UnityEngine;

namespace Samples.Playground2D.Scripts.Tools
{
    public class GravityController2D : ToolController2D
    {
        [Header("Gravity")]
        public bool changeNormalByGround = false;
        
        [Space]
        public bool changeGravity = false;
        [Range(-180, 180)] public float gravityAngle = 0;

        [Space] public float normalChangeRate = 4f;

        [DrawButton()]
        public void ChangeGravity()
        {
            player2d.gravity = Quaternion.Euler(0, 0, gravityAngle) * Vector2.down * player2d.gravity.magnitude;
            player2d.NormalByGravity();
        }

        private void FixedUpdate()
        {
            if (changeGravity)
            {
                player2d.gravity = Quaternion.Euler(0, 0, gravityAngle) * Vector2.down * player2d.gravity.magnitude;
            }

            if (player2d)
            {
                if (changeNormalByGround)
                {
                    if (player2d.grounded)
                    {
                        var playerNormal = Vector2.Lerp(player2d.playerNormal, player2d.groundHit2d.normal, normalChangeRate * Time.fixedDeltaTime);
                        player2d.SetNormal(playerNormal);
                    }
                    else
                    {
                        var playerNormal = Vector2.Lerp(player2d.playerNormal, -player2d.gravityDirection, normalChangeRate * Time.fixedDeltaTime);
                        player2d.SetNormal(playerNormal);
                    }
                }
                else if (changeGravity)
                {
                    var playerNormal = Vector2.Lerp(player2d.playerNormal, -player2d.gravityDirection, normalChangeRate * Time.fixedDeltaTime);
                    player2d.SetNormal(playerNormal);
                }
            }
        }
    }
}