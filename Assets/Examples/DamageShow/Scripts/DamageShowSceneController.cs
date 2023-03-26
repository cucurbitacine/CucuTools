using CucuTools.PlayerSystem;
using Examples.DamageShow.Scripts.Sources;
using UnityEngine;

namespace Examples.DamageShow.Scripts
{
    public class DamageShowSceneController : MonoBehaviour
    {
        public Gun gun;
        public PlayerRigidInput input;
        public DamageMessageGUI logger;
        
        private void Start()
        {
            foreach (var zombie in FindObjectsOfType<ZombieDamageManager>())
            {
                zombie.onDied.AddListener(z =>
                {
                    gun.level++;
                    logger.LogMessage($"Gun level up! [{gun.level}]");
                });
            }
        }

        private void Update()
        {
            if (input.shoot)
            {
                gun.Shoot(new Ray(input.playerCurrent.eyes.position, input.playerCurrent.eyes.forward));
            }
        }
    }
}