using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines
{
    public class StateDebugger : MonoBehaviour
    {
        public bool paused = false;
        
        [Space]
        public string customName = string.Empty;

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
        
        private IState _state;
        
        private string GetStateMachineName(IState sm)
        {
            if (sm == null) return DefaultStateMachineName;
            
            var stateMachineName = customName;

            if (string.IsNullOrWhiteSpace(customName))
            {
                stateMachineName = $"{sm.GetType().Name}";

                if (sm is MonoBehaviour mono)
                {
                    stateMachineName = $"{mono.name} ({stateMachineName})";
                }
            }

            return stateMachineName;
        }

        private string GetStateName(IState state)
        {
            if (state == null) return DefaultStateName;

            var stateName = $"{state.GetType().Name}";

            if (state is MonoBehaviour mono)
            {
                stateName = $"{mono.name} ({stateName})";
            }    
            
            var subStateName = state is IStateMachine stateMachine && stateMachine.SubState != null ? $" > {GetStateName(stateMachine.SubState)}" : "";
            
            return $"{stateName}{subStateName}";
        }

        private void HandleStateUpdate(bool isExecuting)
        {
            if (paused) return;

            if (debugLog)
            {
                Log($"{GetStateMachineName(_state)} :: Executing :: {isExecuting}");
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

        private void Awake()
        {
            TryGetComponent(out _state);
        }
        
        private void OnEnable()
        {
            _state.ExecutionUpdated += HandleStateUpdate;
        }

        private void OnDisable()
        {
            _state.ExecutionUpdated -= HandleStateUpdate;
        }
        
        private void LateUpdate()
        {
            frameNumber++;
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Application.isPlaying && handleLabel && _state != null && _state.IsRunning)
            {
                var position = anchor ? anchor.position : anchor.transform.position + labelOffset;
                var text = string.Empty;

                if (_state is IStateMachine stateMachine)
                {
                    text = $"{GetStateMachineName(stateMachine)} > {GetStateName(stateMachine.SubState)}";
                }
                else
                {
                    text = $"{GetStateName(_state)}";
                }
                
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