using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines
{
    /// <summary>
    /// Simple condition. Pair of key and value
    /// </summary>
    [CreateAssetMenu(menuName = Cucu.AddComponent + Cucu.StateMachineGroup + ObjectName, fileName = ObjectName, order = 0)]
    public class Condition : ScriptableObject
    {
        private const string ObjectName = nameof(Condition);
        
        private UnityEvent<bool> _onValueChanged = new UnityEvent<bool>();

        #region SerializeField

        [SerializeField] private string key = string.Empty;
        [SerializeField] private bool value = false;

        [Space]
        [SerializeField] private bool debugLog = true;
        
        #endregion

        #region Public API

        /// <summary>
        /// Condition key
        /// </summary>
        public string Key
        {
            get => key;
            set => key = value;
        }

        /// <summary>
        /// Condition value
        /// </summary>
        public bool Value
        {
            get => value;
            set => SetValue(value);
        }

        /// <summary>
        /// Condition value change event
        /// </summary>
        public UnityEvent<bool> OnValueChanged => _onValueChanged ??= new UnityEvent<bool>();
        
        /// <summary>
        /// Set new value
        /// </summary>
        /// <param name="newValue"></param>
        public void SetValue(bool newValue)
        {
            if (value == newValue) return;
                
            value = newValue;
            OnValueChanged.Invoke(value);

            if (debugLog) Debug.Log($"\"{Key}\" = {newValue}");
        }
        
        /// <summary>
        /// Set true
        /// </summary>
        public void True()
        {
            SetValue(true);
        }
        
        /// <summary>
        /// Set false
        /// </summary>
        public void False()
        {
            SetValue(false);
        }

        /// <summary>
        /// Toggle value
        /// </summary>
        public void Toggle()
        {
            SetValue(!Value);
        }
        
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                Key = name;
            }
        }
        
        #endregion

        #region MonoBehaviour

        private void OnEnable()
        {
            Validate();
        }

        private void OnValidate()
        {
            Validate();
        }

        #endregion
    }
}