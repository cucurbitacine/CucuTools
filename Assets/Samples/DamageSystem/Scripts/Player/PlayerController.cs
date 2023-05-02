using CucuTools.DamageSystem.Impl;
using UnityEngine;

namespace Samples.DamageSystem.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Settings")]
        public Elemental elementalSelf = Elemental.Fire;
        [Min(1)]
        public int level = 1;

        [Header("References")]
        public DamageManagerExtended damageManager = null;
        public ElementalDamageHandler damageHandler = null;
        public HealthController health = null;

        private void Start()
        {
            damageManager.ReceiverBuffsManager.AddBuff(new PlayerElementalBuff(this, 2));
        }

        private void Update()
        {
            damageHandler.elementalSelf = elementalSelf;
        }
    }
}