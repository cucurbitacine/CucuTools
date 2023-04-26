using CucuTools.DamageSystem;
using UnityEngine;

namespace Samples.DamageSystem.Scripts.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public Elemental elementalSelf = Elemental.Fire;

        [Header("References")]
        public DamageReceiver damageReceiver = null;
        public ElementalDamageHandler damageHandler = null;
        public HealthController health = null;
        
        private void Update()
        {
            damageHandler.elementalSelf = elementalSelf;
        }
    }
}