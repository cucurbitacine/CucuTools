using System.Collections;
using CucuTools.DamageSystem;
using UnityEngine;

namespace Examples.DamageShow.Scripts.Sources
{
    public class Gun : DamageSourceReference
    {
        [Space]
        [Min(0)]
        public int level = 1;

        [Space]
        public float sleepAfterShoot = 1f;
        public LayerMask shootLayers = 1;
        
        [Space]
        public float projectileLife = 0.1f;
        public LineRenderer projectile;

        private Coroutine _projectileLiving = null;

        private bool _sleepGun = false;
        
        public void Shoot(Ray ray)
        {
            if (mute) return;

            if (_sleepGun) return;

            _sleepGun = true;
            
            if (Physics.Raycast(ray, out var hit, 100f, shootLayers))
            {
                Projectile(transform.position, hit.point);
                    
                var rcv = hit.collider.GetComponent<DamageReceiver>();
                if (rcv)
                {
                    var dmg = GenerateDamage(rcv);

                    dmg.point = hit.point;
                    dmg.normal = hit.normal;

                    rcv.ReceiveDamage(dmg);
                }
            }
            else
            {
                Projectile(transform.position, ray.origin + ray.direction * 100);
            }
        }
        
        protected override void HandleDamage(DamageEvent e)
        {
            e.damage.amount += (level - 1);
        }
        
        private void Projectile(Vector3 start, Vector3 end)
        {
            projectile.useWorldSpace = true;

            projectile.positionCount = 2;
            projectile.SetPosition(0, start);
            projectile.SetPosition(1, end);

            if (_projectileLiving != null) StopCoroutine(_projectileLiving);
            _projectileLiving = StartCoroutine(_ProjectileLiving());
        }

        private IEnumerator _ProjectileLiving()
        {
            projectile.enabled = true;

            yield return new WaitForSeconds(projectileLife);

            projectile.enabled = false;
        }

        private IEnumerator _SleepHandle()
        {
            while (true)
            {
                if (_sleepGun)
                {
                    yield return new WaitForSeconds(sleepAfterShoot);
                    _sleepGun = false;
                }

                yield return null;
            }
        }

        private void Start()
        {
            StartCoroutine(_SleepHandle());
        }
    }
}