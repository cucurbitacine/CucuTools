using Examples.DamageShow.Scripts.Sources;
using Examples.Scripts.Input;
using UnityEngine;

namespace Examples.DamageShow.Scripts
{
    public class DamageShowSceneController : MonoBehaviour
    {
        public Gun gun;
        public FirstPersonInput input;
        public DamageMessageGUI logger;
        
        private void Start()
        {
            foreach (var zombie in FindObjectsOfType<ZombieDamageManager>())
            {
                zombie.onDie.AddListener(z =>
                {
                    gun.level++;
                    logger.LogMessage($"Gun level up! [{gun.level}] by kill {z.name}");
                });
            }
        }

        private void Update()
        {
            if (input.data.shoot)
            {
                gun.Shoot(new Ray(input.player.head.position, input.player.head.forward));
            }
        }
    }
}