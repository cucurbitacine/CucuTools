using UnityEngine;

namespace CucuTools.StateMachines
{
    [DisallowMultipleComponent]
    public class StateLauncher : MonoBehaviour
    {
        private IState _state;
        
        private void Awake()
        {
            TryGetComponent(out _state);
        }

        private void Start()
        {
            _state.Enter();
        }

        private void Update()
        {
            _state.Execute();
        }

        private void OnDestroy()
        {
            _state.Exit();
        }
    }
}