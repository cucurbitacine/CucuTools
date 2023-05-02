using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.DamageSystem.Buffs
{
    [CreateAssetMenu(menuName = "Create DamageEffectManagerFactory", fileName = "DamageEffectManagerFactory", order = 0)]
    public class DamageBuffsProfile : ScriptableObject
    {
        [SerializeField] private List<DamageBuffFactory> buffFactories = new List<DamageBuffFactory>();

        public DamageBuffManager CreateManager()
        {
            var manager = new DamageBuffManager();

            foreach (var buffFactory in buffFactories)
            {
                if (buffFactory != null) manager.AddBuff(buffFactory.CreateBuff());
            }

            return manager;
        }

        public static DamageBuffsProfile CreateEmpty()
        {
            return CreateInstance<DamageBuffsProfile>();
        }
    }
}