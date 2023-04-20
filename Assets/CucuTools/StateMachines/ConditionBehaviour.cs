using System;
using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines
{
    /// <summary>
    /// Behaviour representing condition 
    /// </summary>
    public class ConditionBehaviour : CucuBehaviour
    {
        public UnityEvent<bool> onValueChanged = new UnityEvent<bool>();

        public bool updateOnStart = true;
        
        [Header("Condition")]
        [ReadOnly]
        [SerializeField] private string key;
        [ReadOnly]
        [SerializeField] private bool value;
        
        [Space]
        public Condition condition = null;
        
        public string Key => condition.Key;
        public bool Value => condition.Value;
        
        public void SetValue(bool newValue)
        {
            condition.Value = newValue;
        }
        
        [DrawButton("True", group:"Condition")]
        public void True()
        {
            SetValue(true);
        }
        
        [DrawButton("False", group:"Condition")]
        public void False()
        {
            SetValue(false);
        }

        [DrawButton("Toggle", group:"Condition")]
        public void Toggle()
        {
            SetValue(!Value);
        }

        private void UpdateBehaviour(bool conditionValue)
        {
#if UNITY_EDITOR
            key = Key;
            value = conditionValue;
#endif
            
            onValueChanged.Invoke(conditionValue);
        }
        
        private void Awake()
        {
            if (condition == null)
            {
                throw new Exception($"{nameof(ConditionBehaviour)} \"{name}\" has empty condition!");
            }
        }

        private void OnEnable()
        {
            condition.OnValueChanged.AddListener(UpdateBehaviour);
        }

        private void Start()
        {
            if (updateOnStart) UpdateBehaviour(Value);
        }

        private void OnDisable()
        {
            condition.OnValueChanged.RemoveListener(UpdateBehaviour);
        }

        private void OnValidate()
        {
            if (condition != null)
            {
                key = Key;
                value = Value;
            }
        }
    }
}
