using CucuTools.DamageSystem;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts
{
    public class Gun : DamageSource
    {
        [Header("Gun")]
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform firePoint;

        [ContextMenu(nameof(Fire))]
        public void Fire()
        {
            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            bullet.DamageBox.SetDamageSource(this);
        }
    }
}