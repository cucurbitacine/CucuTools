using CucuTools.PlayerSystem;
using Examples.DamageShow.Scripts.Sources;
using UnityEngine;

namespace Examples.DamageShow.Scripts
{
    public class DamageShowSceneController : MonoBehaviour
    {
        public Gun gun;
        public PlayerInput input;
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
                gun.Shoot(new Ray(input.person.eyes.position, input.person.eyes.forward));
            }
        }
    }
}