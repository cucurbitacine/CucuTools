using UnityEngine;

namespace CucuTools.DamageSystem
{
    [CreateAssetMenu(menuName = "CucuTools/Damage System/Damage Asset", fileName = "Damage", order = 0)]
    public class DamageAsset : ScriptableObject
    {
        [SerializeField] private int damageAmount = 1;
        [Min(0)]
        [SerializeField] private int damageThreshold = 0;
        
        public virtual DamageData Generate(DamageAgent owner, GameObject attackObject, DamageAgent target, GameObject hitObject)
        {
            var amount = damageAmount;
            
            if (damageThreshold > 0)
            {
                amount += Random.Range(0, damageThreshold + 1);
            }

            return new DamageData(amount);
        }
    }
}