using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.DamageSystem
{
    public abstract class DamageEffector : MonoBehaviour, IDamageEffect
    {
        public abstract bool IsEnabled { get; set; }

        private List<IDamageEffect> Effects { get; } = new List<IDamageEffect>();
        
        public void AddEffect(params IDamageEffect[] filters)
        {
            Effects.AddRange(filters.Where(m => !Effects.Contains(m)));
        }
        
        public void RemoveEffect(params IDamageEffect[] filters)
        {
            Effects.RemoveAll(filters.Contains);
        }

        public DamageInfo Evaluate(DamageInfo damage)
        {
            foreach (var modifier in Effects)
            {
                if (modifier.IsEnabled) damage = modifier.Evaluate(damage);
            }
            
            return damage;
        }
    }
    
    public abstract class DamageEffect : IDamageEffect
    {
        [SerializeField] private bool isEnabled = true;

        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        public abstract DamageInfo Evaluate(DamageInfo damage);
    }
    
    public interface IDamageEffect
    {
        bool IsEnabled { get; set; }
        DamageInfo Evaluate(DamageInfo damage);
    }
}