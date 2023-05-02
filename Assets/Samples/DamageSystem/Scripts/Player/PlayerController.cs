using CucuTools.DamageSystem.Buffs.Impl;
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
            damageManager.ReceiverBuffsManager.onAddedBuff += buff => Debug.Log($"Added {buff.GetType().Name}");
            damageManager.ReceiverBuffsManager.onRemovedBuff += buff => Debug.Log($"Removed {buff.GetType().Name}");
            
            damageManager.ReceiverBuffsManager.AddBuff(new PlayerElementalBuff(this, 2));

            var addBuff = new DamageAddBuff(1);
            var tempBuff = new DamageBuffTimer(addBuff, 3f);
            damageManager.ReceiverBuffsManager.AddBuff(tempBuff);
        }

        private void Update()
        {
            damageHandler.elementalSelf = elementalSelf;
        }
    }
}