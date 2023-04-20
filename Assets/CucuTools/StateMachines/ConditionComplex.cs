using System;
using System.Linq;
using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines
{
    public class ConditionComplex : CucuBehaviour
    {
        public UnityEvent<bool> onValueChanged = new UnityEvent<bool>();

        public bool updateOnStart = true;
        
        [Header("Condition")]
        [ReadOnlyField]
        [SerializeField] private bool value = false;

        [Space]
        public Mode mode = Mode.All;
        public Condition[] conditions = null;

        public bool Value => mode == Mode.All ? conditions.All(c => c.Value) : conditions.Any(c => c.Value);

        private bool _previousValue = false;
        
        private void AnyConditionChanged(bool v)
        {
            var newValue = Value;

            if (_previousValue == newValue) return;
            
            UpdateBehaviour(newValue);
        }
        
        private void UpdateBehaviour(bool conditionValue)
        {
            _previousValue = conditionValue;
            
#if UNITY_EDITOR
            value = conditionValue;
#endif
            
            onValueChanged.Invoke(conditionValue);
        }

        private void AddListeners()
        {
            _previousValue = Value;
            
            foreach (var condition in conditions)
            {
                condition.OnValueChanged.AddListener(AnyConditionChanged);
            }
        }

        private void RemoveListeners()
        {
            foreach (var condition in conditions)
            {
                condition.OnValueChanged.RemoveListener(AnyConditionChanged);
            }
        }

        private void Awake()
        {
            if (conditions == null || conditions.Length == 0)
            {
                throw new Exception($"{nameof(ConditionBehaviour)} \"{name}\" has empty condition!");
            }
        }

        private void OnEnable()
        {
            AddListeners();
        }

        private void Start()
        {
            if (updateOnStart) UpdateBehaviour(Value);
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void OnValidate()
        {
            if (conditions != null) value = Value;
        }

        public enum Mode
        {
            All,
            Any,
        }
    }
}