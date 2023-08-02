using CucuTools.DamageSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.DamageSystem.Scripts
{
    public class DamageLog : MonoBehaviour
    {
        public bool active = true;
        
        [Space]
        public Text logField;

        public string BuildMessage(DamageEvent e)
        {
            var rcvMsg = e.receiver.name;

            if (e.receiver.manager)
            {
                rcvMsg = $"{e.receiver.manager.name} ({rcvMsg})";
            }

            var dmgMsg = $"{e.damage.amount}{(e.damage.critical?" CRITICAL":"")}";
            
            if (e.damage is ElementalDamage eDamage)
            {
                dmgMsg = $"{dmgMsg} {eDamage.elemental}";
            }
            
            var srcMsg = e.source.name;
            
            if (e.source.manager)
            {
                srcMsg = $"{e.source.manager.name} ({srcMsg})";
            }

            return $"\"{rcvMsg}\" received \"{dmgMsg}\" damage from \"{srcMsg}\"";
        }

        private int _hitCount = 0;
        
        public void Log(DamageEvent e)
        {
            var msg = BuildMessage(e);

            if (active) Debug.Log($">>> Damage :: {msg}");

            if (logField)
            {
                if (logField.text.StartsWith(msg))
                {
                    _hitCount++;

                    logField.text = $"{msg} x{(_hitCount + 1)}";
                }
                else
                {
                    logField.text = msg;
                    _hitCount = 0;
                }
            }
        }
        
        private void Awake()
        {
            Damage.Event += Log;
        }
    }
}