using UnityEngine;

namespace CucuTools.StateMachines.Impl
{
    public class StateMachineDebugger : CucuBehaviour
    {
        public bool paused = false;
        
        [Space]
        public string customName = string.Empty;
        public StateMachineBase stateMachine;
        
        [Space]
        public bool debugLog = true;
        
        [Space]
        public bool handleLabel = true;
        public Vector3 labelOffset = new Vector3(0.1f, -0.2f, 0.0f);
        public Color labelColor = new Color(0.9f, 0.9f, 0.9f, 0.8f);
        public Color labelBackground = new Color(0.1f, 0.1f, 0.1f, 0.2f);

        private const string DefaultStateMachineName = "???";
        private const string DefaultStateName = "-";
        
        private GUIStyle _labelStyle;
        private Texture2D _background;
        
        private string GetStateMachineName(StateMachineBase sm)
        {
            if (sm == null) return DefaultStateMachineName;
            
            var stateMachineName = customName;

            if (string.IsNullOrWhiteSpace(customName))
            {
                stateMachineName = $"{sm.name} ({sm.GetType().Name})";
            }

            return stateMachineName;
        }

        private string GetStateName(StateBase state)
        {
            if (state == null) return DefaultStateName;
            
            var stateName = $"{state.name} ({state.GetType().Name})";
            var subStateName = state.subState ? $" > {GetStateName(state.subState)}" : "";
            
            return $"{stateName}{subStateName}";
        }

        private void StateUpdated(StateEventType eventType)
        {
            if (paused) return;

            if (debugLog && eventType != StateEventType.Update)
            {
                Debug.Log($"{GetStateMachineName(stateMachine)} :: Event ::{eventType}");
            }
        }

        private void StateChanged(StateBase nextState)
        {
            if (paused) return;
            
            if (debugLog && stateMachine)
            {
                var activeState = stateMachine.activeState;
                var lastState = stateMachine.lastState;
                
                Debug.Log($"{GetStateMachineName(stateMachine)} :: Change :: [{GetStateName(lastState)}] => [{GetStateName(activeState)}]");
            }
        }

        private void Validate()
        {
            if (stateMachine == null) stateMachine = GetComponent<StateMachineBase>();
        }

        private void Awake()
        {
            Validate();
        }

        private void OnEnable()
        {
            if (stateMachine)
            {
                stateMachine.onUpdated.AddListener(StateUpdated);
                stateMachine.onStateChanged.AddListener(StateChanged);
            }
        }

        private void OnDisable()
        {
            if (stateMachine)
            {
                stateMachine.onUpdated.RemoveListener(StateUpdated);
                stateMachine.onStateChanged.RemoveListener(StateChanged);
            }
        }

        private void OnValidate()
        {
            Validate();
        }
        
        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Application.isPlaying && handleLabel && stateMachine && stateMachine.isActive)
            {
                var position = stateMachine.transform.position + labelOffset;
                var text = GetStateName(stateMachine.activeState);

                if (_labelStyle == null)
                {
                    _labelStyle = new GUIStyle(GUI.skin.label);
                    
                    _background = new Texture2D(1, 1, TextureFormat.RGBA64, false);
                    _labelStyle.normal.background = _background;
                }

                _labelStyle.normal.textColor = labelColor;

                _background.SetPixel(0, 0, labelBackground);
                _background.Apply();
                
                UnityEditor.Handles.Label(position, text, _labelStyle);
            }
#endif
        }
    }
}