using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Others
{
    public class CucuTrigger : MonoBehaviour
    {
        public TriggerMode mode = TriggerMode.Enter;
        public LayerMask layerMask = 1;
        
        [Space] public UnityEvent<Collider> onChanged = new UnityEvent<Collider>();

        public List<Collider> whiteList = new List<Collider>();

        [SerializeField] private bool debugLog = true;
        
        private void Invoke(Collider other)
        {
            if (debugLog) Debug.Log($"[{name}] On Trigger {mode} with [{other.name}]");
            
            onChanged.Invoke(other);
        }

        private void OnTrigger(TriggerMode triggerMode, Collider other)
        {
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