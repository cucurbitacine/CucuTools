using CucuTools;
using CucuTools.DamageSystem;
using CucuTools.Others;
using UnityEngine;

namespace Samples.DamageSystem.Scripts.Player
{
    public class PlayerDamageSource : ElementalDamageSource
    {
        [Header("Player Settings")]
        public LayerMask layerTargets = 1;

        private readonly CachedComponent<Collider, DamageReceiver> receivers =
            new CachedComponent<Collider, DamageReceiver>();

        public Camera cam => Camera.main;
        public Ray ray => cam.ScreenPointToRay(Input.mousePosition);
        
        public void Attack()
        {
            Debug.Log("Attack");
            
            if (Physics.Raycast(ray, out var hit))
            {
                Debug.Log($"Hit {hit.collider.name}");

                if (CheckHit(hit, out var receiver))
                {
                    Debug.Log($"Send Damage");

                    SendDamage(receiver);
                }
            }
        }
        
        private bool CheckLayer(int value)
        {
            return value.Contains(layerTargets);
        }

        private bool TryGetReceiver(Collider cld, out DamageReceiver receiver)
        {
            return receivers.TryGetValidValue(cld, out receiver);
        }

        private bool CheckManager(DamageReceiver receiver)
        {
            return manager == null || receiver.manager == null || manager != receiver.manager;
        }

        private bool CheckHit(RaycastHit hit, out DamageReceiver receiver)
        {
            receiver = default;
            
            return CheckLayer(hit.collider.gameObject.layer) &&
                   TryGetReceiver(hit.collider, out receiver) &&
                   CheckManager(receiver);
        }
    }
}