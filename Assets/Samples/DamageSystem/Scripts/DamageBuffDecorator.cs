using System.Collections;
using CucuTools;
using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Buffs;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    public abstract class DamageBuffDecorator : DamageBuff
    {
        private readonly DamageBuff buff;

        public DamageBuffDecorator(DamageBuff buff)
        {
            this.buff = buff;
        }
        
        public override void HandleDamage(DamageEvent e)
        {
            buff.HandleDamage(e);
        }

        public override void Start(DamageBuffManager buffManager)
        {
            buff.Start(buffManager);
        }

        public override void Stop(DamageBuffManager buffManager)
        {
            buff.Stop(buffManager);
        }
    }
    
    public sealed class DamageBuffTimer : DamageBuffDecorator
    {
        public readonly float duration;
        
        private Coroutine _coroutine;
        
        public DamageBuffTimer(DamageBuff buff, float duration) : base(buff)
        {
            this.duration = duration;
        }
        
        public override void Start(DamageBuffManager buffManager)
        {
            base.Start(buffManager);
            
            if (_coroutine != null) Cucu.StopCoroutine(_coroutine);
            _coroutine = Cucu.StartCoroutine(Processing(buffManager));
        }

        public override void Stop(DamageBuffManager buffManager)
        {
            if (_coroutine != null) Cucu.StopCoroutine(_coroutine);
            
            base.Stop(buffManager);
        }

        private IEnumerator Processing(DamageBuffManager buffManager)
        {
            yield return new WaitForSeconds(duration);
            
            buffManager.RemoveBuff(this);
        }
    }
}