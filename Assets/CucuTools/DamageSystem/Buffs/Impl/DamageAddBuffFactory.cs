using System;
using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.DamageSystem.Buffs.Impl
{
    [CreateAssetMenu(menuName = CreateEffectMenu + AssetName, fileName = AssetName, order = 0)]
    public class DamageAddBuffFactory : DamageBuffFactory
    {
        public const string AssetName = nameof(DamageAddBuffFactory);

        public bool createInstance = true;
        public DamageAddBuff buffSeed = new DamageAddBuff();
        
        public override DamageBuff CreateBuff()
        {
            return createInstance ? new DamageAddBuff(buffSeed) : buffSeed;
        }
        
#if UNITY_EDITOR
        [Header("Debug")]
        [Min(0)] [SerializeField] private int damageInput = 1;
        [ReadOnlyField] [SerializeField] private int damageOutput = 1;

        private void OnValidate()
        {
            damageOutput = buffSeed.Add(damageInput);
        }
#endif
    }

    [Serializable]
    public class DamageAddBuff : DamageBuff
    {
        public int addition = 0;
        
        [Space]
        public bool canBeZero = true;

        public DamageAddBuff()
        {
        }
        
        public DamageAddBuff(DamageAddBuff addBuff)
        {
            addition = addBuff.addition;
            canBeZero = addBuff.canBeZero;
        }

        public int Add(int value)
        {
            var result = value + addition;
            
            return Mathf.Max(result, canBeZero ? 0 : 1);
        }
        
        public override void HandleDamage(DamageEvent e)
        {
            e.damage.amount = Add(e.damage.amount);
        }
    }
}