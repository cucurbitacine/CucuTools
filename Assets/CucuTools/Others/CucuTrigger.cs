using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Others
{
    public class CucuTrigger : MonoBehaviour
    {
        public bool isEnabled = true;
        public bool invokeOnce = false;
        public TriggerMode mode = TriggerMode.Enter;
        public LayerMask layerMask = 1;
        
        [Space] public UnityEvent<Collider> onChanged = new UnityEvent<Collider>();

        public List<Collider> whiteList = new List<Collider>();

        [SerializeField] private bool debugLog = true;

        public void SetEnable(bool value)
        {
            isEnabled = value;
        }
        
        public void SwitchEnable()
        {
            SetEnable(!isEnabled);
        }

        public void Enable()
        {
            SetEnable(true);
        }
        
        public void Disable()
        {
            SetEnable(false);
        }
        
        private void Invoke(Collider other)
        {
            if (debugLog) Debug.Log($"[{name}] On Trigger {mode} with [{other.name}]");
            
            onChanged.Invoke(other);

            if (invokeOnce) Disable();
        }

        private void OnTrigger(TriggerMode triggerMode, Collider other)
        {
            if (!isEnabled) return;
            if (mode != triggerMode) return;

            if (whiteList.Count > 0)
            {
                if (whiteList.Contains(other))
                {
                    Invoke(other);
                }
            }
            else if (layerMask.Contains(other.gameObject.layer))
            {
                Invoke(other);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTrigger(TriggerMode.Enter, other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTrigger(TriggerMode.Stay, other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTrigger(TriggerMode.Exit, other);
        }
    }

    public enum TriggerMode
    {
        Enter,
        Stay,
        Exit,
    }
}