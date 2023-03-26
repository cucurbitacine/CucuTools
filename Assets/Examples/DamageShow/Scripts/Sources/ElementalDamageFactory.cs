using CucuTools.DamageSystem;
using UnityEngine;

namespace Examples.DamageShow.Scripts.Sources
{
    [CreateAssetMenu(menuName = "Examples/Damage/" + nameof(ElementalDamageFactory), fileName = nameof(ElementalDamageFactory), order = 0)]
    public class ElementalDamageFactory : DamageFactory
    {
        [Space] public DamageTemplate template = new DamageTemplate();
        
        public override Damage CreateDamage()
        {
            Damage dmg;
            
            switch (Random.Range(0, 5))
            {
                case 0: dmg = new FireDamage(); break;
                case 1: dmg = new WaterDamage(); break;
                case 2: dmg = new AirDamage(); break;
                case 3: dmg = new EarthDamage(); break;
                case 4: dmg = new LightningDamage(); break;
                default: dmg = new Damage(); break;
            }

            template.Apply(dmg);

            return dmg;
        }
    }
}