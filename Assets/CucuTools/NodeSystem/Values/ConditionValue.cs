using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.NodeSystem.Values
{
    public class ConditionValue : ValueObject, IConditionCollection, IReady 
    {        
        [Header("Conditions")]
        [SerializeField] private List<Condition> _condition = new List<Condition>();
        [Space]
        [SerializeField] private bool _anyTrue = false;
        [SerializeField] private bool _mute = false;
        [SerializeField] private bool _solo = false;

        public override ValueType GetValueType()
        {
            return ValueType.Bool;
        }

        public override ValueData GetData(INode node)
        {
            return new ValueData(IsReady(node));
        }
        
        public ICollection<Condition> GetConditions()
        {
            return _condition;
        }
        
        public bool IsReady(INode node)
        {
            if (_mute) return false;
            if (_solo) return true;
            
            return _anyTrue ? GetConditions().Any(c => c.IsReady(node)) : GetConditions().All(c => c.IsReady(node));
        }
    }
}