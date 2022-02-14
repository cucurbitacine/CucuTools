using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    public sealed class DamageLogger : MonoBehaviour
    {
        private static DamageLogger Instance { get; set; }
        
        public bool isOn = true;
        
        [Space]
        public bool onlyGenerated = false;
        public bool onlyApplied = false;

        public void OnGenerated(DamageEvent e)
        {
            if (isOn && !onlyApplied) Log(GenerateMessage(e));
        }

        public void OnApplied(DamageEvent e)
        {
            if (isOn && !onlyGenerated) Log(ApplyMessage(e));
        }

        public void Log(string msg)
        {
            Debug.Log($"{msg}");
        }

        private string DamageMessage(DamageInfo damage)
        {
            return $"{damage.amount} {damage.type}{(damage.isCritical ? " CRITICAL" : "")}";
        }

        private string GenerateMessage(DamageEvent e)
        {
            if (e.source != null)
                return $"[{e.source.name}] generated [{DamageMessage(e.damage)}] for [{e.receiver.name}]";
            
            return $"Generated [{DamageMessage(e.damage)}] for [{e.receiver.name}]";
        }

        private string ApplyMessage(DamageEvent e)
        {
            if (e.source != null)
                return $"[{e.receiver.name}] applied [{DamageMessage(e.damage)}] by [{e.source.name}]";
            
            return $"[{e.receiver.name}] applied [{DamageMessage(e.damage)}]";
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            DamageEventManager.OnDamageGenerated += OnGenerated;
            DamageEventManager.OnDamageApplied += OnApplied;
        }

        private void OnDisable()
        {
            DamageEventManager.OnDamageGenerated -= OnGenerated;
            DamageEventManager.OnDamageApplied -= OnApplied;
        }
    }
}