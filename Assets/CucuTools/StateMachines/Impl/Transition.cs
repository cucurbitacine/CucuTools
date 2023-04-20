using System;
using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.StateMachines.Impl
{
    /// <inheritdoc />
    [AddComponentMenu(Cucu.AddComponent + Cucu.StateMachineGroup + ComponentName, 1)]
    public class Transition : TransitionBase
    {
        private const string ComponentName = "Transition";
        
        private StateMachineBase _stateMachine;

        #region SerializeField

        [Header("Condition")]
        [Tooltip("If \"conditionBase\" is empty, it will be created with below key and value. Else below key will be overwritten")]
        [SerializeField] private string conditionKey = string.Empty;
        
        [Space]
        [SerializeField] private Condition condition = null;
        
        [Header("Settings")]
        [SerializeField] private StateBase targetState = null;

        [Space]
        [Tooltip("If true, then transition will be always valid")]
        [SerializeField] private bool withoutCondition = false;
        [Tooltip("If true, then after transition condition value will be tried reset to false")]
        [SerializeField] private bool resetCondition = true;

        #endregion

        #region Public API

        public Condition Condition
        {
            get => condition;
            set => condition = value;
        }
        
        public bool WithoutCondition
        {
            get => withoutCondition;
            set => withoutCondition = value;
        }
        
        public bool ResetCondition
        {
            get => resetCondition;
            set => resetCondition = value;
        }

        #endregion
        
        #region TransitionBase

        public override string ConditionKey
        {
            get
            {
                if (Condition != null)
                {
                    conditionKey = Condition.Key;
                }

                return conditionKey;
            }
            set
            {
                conditionKey = value;
                
                if (Condition != null)
                {
                    Condition.Key = conditionKey;
                }
            }
        }

        public override bool ConditionValue
        {
            get
            {
                if (WithoutCondition) return true;
                
                if (Condition != null)
                {
                    return Condition.Value;
                }

                return false;
            }
            set
            {
                if (Condition != null)
                {
                    Condition.Value = value;
                }
            }
        }

        /// <inheritdoc />
        public override StateBase TargetState
        {
            get => targetState;
            set => targetState = value;
        }

        /// <inheritdoc />
        public override bool NextState(out StateBase state)
        {
            state = TargetState;

            if (ConditionValue)
            {
                if (ResetCondition)
                {
                    ConditionValue = false;
                }

                return true;
            }
            
            return false;
        }

        public override void Validate()
        {
            if (WithoutCondition)
            {
                Condition = null;
                conditionKey = "";
            }
            else if (string.IsNullOrWhiteSpace(ConditionKey))
            {
                ConditionKey = $"{Guid.NewGuid()}";
            }
            
            if (string.IsNullOrWhiteSpace(Key))
            {
                Key = $"{nameof(Transition)}-{Guid.NewGuid()}";
            }

            _stateMachine = GetComponentInParent<StateMachineBase>();

            UpdateTransitionName();
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            Validate();
        }

        private void Start()
        {
            if (!WithoutCondition)
            {
                if (_stateMachine is StateMachine sm)
                {
                    if (Condition == null)
                    {
                        Condition = sm.ConditionContext.GetOrCreate(conditionKey);
                    }
                    else
                    {
                        sm.ConditionContext.Add(Condition);
                        conditionKey = Condition.Key;
                    }
                }
            }
        }
        
        private void OnValidate()
        {
            Validate();
        }

        #endregion
        
        [DrawButton("Update Name", group: "Transition")]
        private void UpdateTransitionName()
        {
#if UNITY_EDITOR
            var nextStateName = targetState?.Key ?? "";

            gameObject.name = $"-> {nextStateName} : {ConditionKey} ?";
#endif
        }
    }
}