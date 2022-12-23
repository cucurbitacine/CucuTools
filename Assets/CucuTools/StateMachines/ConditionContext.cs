using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.StateMachines
{
    /// <summary>
    /// Context of conditions. Contains list of conditions
    /// </summary>
    [CreateAssetMenu(menuName = Cucu.AddComponent + Cucu.StateMachineGroup + ObjectName, fileName = ObjectName, order = 0)]
    public class ConditionContext : ScriptableObject
    {
        public const string ObjectName = nameof(ConditionContext);

        private readonly Dictionary<string, Condition> _conditionsDict = new Dictionary<string, Condition>();

        [SerializeField] private string listName = nameof(ConditionContext);
        
        [Space]
        [SerializeField] private List<Condition> conditions = new List<Condition>();

        public string ListName => listName;
        
        public List<Condition> Conditions => conditions ??= new List<Condition>();
        
        /// <summary>
        /// Contains condition with key or not
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Contains(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception($"Condition Key cannot be null or white space");
            }

            return Conditions.Any(c => string.Equals(c.Key, key));
        }
        
        /// <summary>
        /// Get Condition with key, if it is not exist - creating it
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Condition GetOrCreate(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception($"Condition Key cannot be null or white space");
            }

            if (_conditionsDict.TryGetValue(key, out var cnd))
            {
                return cnd;
            }
            
            var condition = Conditions.FirstOrDefault(c => string.Equals(c.Key, key));

            if (condition != null)
            {
                _conditionsDict.Add(key, condition);
                return condition;
            }
            
            var newCondition = CreateInstance<Condition>();
            newCondition.Key = key;
            newCondition.Value = false;
            newCondition.name = key;
            
            Conditions.Add(newCondition);
            _conditionsDict.Add(key, newCondition);

            return newCondition;
        }
        
        /// <summary>
        /// Set condition value by key. If it is not exist - creating new one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetOrCreate(string key, bool value)
        {
            var condition = GetOrCreate(key);
            condition.Value = value;
        }
        
        /// <summary>
        /// Add condition. If it is already exist - do nothing
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool Add(Condition condition)
        {
            if (Conditions.Contains(condition)) return false;

            Conditions.Add(condition);

            return true;
        }
    }
}