using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines
{
    public class StateMachineDebugger : MonoBehaviour
    {
        public bool paused = false;
        
        [Space]
        public string customName = string.Empty;
        public StateMachineBase stateMachine;

        [Space]
        public bool debugLog = true;
        public bool showFrameNumber = false;

        [Space]
        public Transform anchor;
        public bool handleLabel = true;
        public Vector3 labelOffset = new Vector3(0.1f, -0.2f, 0.0f);
        public Color labelColor = new Color(0.9f, 0.9f, 0.9f, 0.8f);
        public Color labelBackground = new Color(0.1f, 0.1f, 0.1f, 0.2f);

        private const string DefaultStateMachineName = "???";
        private const string DefaultStateName = "-";
        
        private GUIStyle _labelStyle;
        private Texture2D _background;
        
        private int frameNumber;
        
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
            var subStateName = state.SubState ? $" > {GetStateName(state.SubState)}" : "";
            
            return $"{stateName}{subStateName}";
        }

        private void HandleStateUpdate(StateEventType eventType)
        {
            if (paused) return;

            if (debugLog)
            {
                Log($"{GetStateMachineName(stateMachine)} :: Updated :: {eventType}");
            }
        }

        private void HandleStateChange(StateBase nextState)
        {
            if (paused) return;
            
            if (debugLog && stateMachine)
            {
                var activeState = stateMachine.ActiveState;
                var lastState = stateMachine.PreviousState;
                
                Log($"{GetStateMachineName(stateMachine)} :: Changed :: [{GetStateName(lastState)}] => [{GetStateName(activeState)}]");
            }
        }

        private void Log(string msg)
        {
            if (showFrameNumber)
            {
                Debug.Log($"[{frameNumber}] {msg}");
            }
            else
            {
                Debug.Log($"{msg}");
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
        
        private void OnValidate()
        {
            Validate();
        }
        
        private void OnEnable()
        {
            if (stateMachine)
            {
                stateMachine.OnStateUpdated += HandleStateUpdate;
                stateMachine.OnStateChanged += HandleStateChange;
            }
        }

        private void OnDisable()
        {
            if (stateMachine)
            {
                stateMachine.OnStateUpdated -= HandleStateUpdate;
                stateMachine.OnStateChanged -= HandleStateChange;
            }
        }
        
        private void LateUpdate()
        {
            frameNumber++;
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Application.isPlaying && handleLabel && stateMachine && stateMachine.actAsStateMachine && stateMachine.isActive)
            {
                var position = anchor ? anchor.position : stateMachine.transform.position + labelOffset;
                var text = $"{GetStateMachineName(stateMachine)} > {GetStateName(stateMachine.ActiveState)}";

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