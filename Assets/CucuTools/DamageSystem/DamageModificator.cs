using UnityEngine;

namespace CucuTools.DamageSystem
{
    public abstract class DamageModificator : MonoBehaviour
    {
        public abstract int ModificatorOrder { get; }
        public abstract void Modify(DamageProcess process);
    }
    
    public abstract class DamageAttackModificator : DamageModificator
    {
        [field: SerializeField] public DamageAttackHandler AttackHandler { get; private set; }
        [SerializeField] private int modificatorOrder;
        
        public override int ModificatorOrder => modificatorOrder;
        
        public void ChangeAttackHandler(DamageAttackHandler attackHandler)
        {
            AttackHandler = attackHandler;
        }
        
        protected virtual void OnEnable()
        {
            AttackHandler.AddModificator(this);
        }
        
        protected virtual void OnDisable()
        {
            AttackHandler.RemoveModificator(this);
        }
    }
    
    public abstract class DamageHitModificator : DamageModificator
    {
        [field: SerializeField] public DamageHitHandler HitHandler { get; private set; }
        [SerializeField] private int modificatorOrder;
        
        public override int ModificatorOrder => modificatorOrder;
        
        public void ChangeHitHandler(DamageHitHandler hitHandler)
        {
            HitHandler = hitHandler;
        }
        
        protected virtual void OnEnable()
        {
            HitHandler.AddModificator(this);
        }
        
        protected virtual void OnDisable()
        {
            HitHandler.RemoveModificator(this);
        }
    }
}