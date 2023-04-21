using System.Collections.Generic;
using CucuTools;
using CucuTools.DamageSystem;

namespace Examples.Playground.Scripts
{
    public class DamagingObjectManager : CucuBehaviour
    {
        public DamageFactory factoryDefault = null;
        
        private readonly HashSet<DamagingObject> _hash = new HashSet<DamagingObject>();

        private void Awake()
        {
            foreach (var damagingObject in FindObjectsOfType<DamagingObject>())
            {
                if (factoryDefault != null && damagingObject.factory == null)
                {
                    damagingObject.factory = factoryDefault;
                }
                
                _hash.Add(damagingObject);
            }
        }
    }
}