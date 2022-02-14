using CucuTools.Avatar;
using CucuTools.DamageSystem.Impl;
using UnityEngine;

namespace Example.Scripts.DamageSystem
{
    public class CucuDamageShooter : DamageSourceSimple
    {
        [Space]
        public CucuAvatar Avatar;
        
        public Transform Head => Avatar.Head;
        public Ray Ray => new Ray(Head.position, Head.forward);

        public void Shoot()
        {
            var hits = Physics.RaycastAll(Ray);

            foreach (var hit in hits)
            {
                var hitBox = hit.collider.GetComponent<HitBox>();
                if (hitBox == null) continue;
                
                hitBox.Hit(this);
            }
        }
        
        private void Update()
        {
            if (IsEnabled && Input.GetKeyDown(KeyCode.Mouse0)) Shoot();
        }
    }
}