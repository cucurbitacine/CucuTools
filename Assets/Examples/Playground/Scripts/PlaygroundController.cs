using CucuTools.PlayerSystem;
using CucuTools.Scenes;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    [SceneController]
    public class PlaygroundController : SceneController
    {
        public PlayerRigidController player;

        public Transform startPoint;

        public void TeleportToStartPoint()
        {
            player.transform.position = startPoint.position;
        }
    }
}
