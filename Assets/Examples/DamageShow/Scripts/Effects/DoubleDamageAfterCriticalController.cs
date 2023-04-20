using System.Collections;
using CucuTools.Attributes;
using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Extended;
using CucuTools.DamageSystem.Extended.Effects.Impl;
using CucuTools.Terminal;
using UnityEngine;

namespace Examples.DamageShow.Scripts.Effects
{
    public class DoubleDamageAfterCriticalController : TerminalCommandRegistrator
    {
        public bool isActive = false;
        
        [Space]
        public float duration = 5;

        [Space] public DamageSourceExtended source;

        private DamageMultiplierEffect _doubleDamage = null;
        private CriticalDamageHandlerEffect _damageHandler = null;
        
        private Coroutine _double = null;
        
        [DrawButton()]
        [TerminalCommand("damage.double")]
        public void Double()
        {
            if (_double != null) StopCoroutine(_double);
            _double = StartCoroutine(_Double());
        }

        [TerminalCommand("damage.duration")]
        private void SetDuration(float dur)
        {
            duration = dur;
            
            Debug.Log($"Set double duration = {duration}");
        }
        
        private IEnumerator _Double()
        {
            isActive = true;
            
            source.effects.AddEffect(_doubleDamage);
            yield return new WaitForSeconds(duration);
            source.effects.RemoveEffect(_doubleDamage);
            
            isActive = false;
        }
        
        private void CriticalDamage(DamageEvent e)
        {
            Debug.Log("Critical damage! Add double damage effect!");
            
            Double();
        }
        
        private void Start()
        {
            _doubleDamage = ScriptableObject.CreateInstance<DamageMultiplierEffect>();
            _doubleDamage.factor = 2;

            _damageHandler = ScriptableObject.CreateInstance<CriticalDamageHandlerEffect>();
            _damageHandler.onCriticalDamaged.AddListener(CriticalDamage);
            
            source.effects.AddEffect(_damageHandler);
        }
    }
}