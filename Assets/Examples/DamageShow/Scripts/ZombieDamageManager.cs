using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Extended;
using Examples.DamageShow.Scripts.Sources;
using UnityEngine;
using UnityEngine.Events;

namespace Examples.DamageShow.Scripts
{
    public class ZombieDamageManager : DamageManagerExtended
    {
        [Space] public bool dead = false;
        public float impulseDamageScale = 10;

        [Space] public UnityEvent<ZombieDamageManager> onDie = new UnityEvent<ZombieDamageManager>();

        [Space] public ZombieAI ai;
        [Space] public HealthManager health;

        public ParticleSystem _bloodVFX;
        
        public void Die()
        {
            if (dead) return;
            
            dead = true;
            
            ai.enabled = false;
            ai.personTyped.Stop();
            ai.personTyped.capsule.sharedMaterial = null;
            ai.personTyped.rigid.useGravity = true;
            ai.personTyped.rigid.constraints = RigidbodyConstraints.None;
            
            Destroy(ai.personTyped);
            
            onDie.Invoke(this);
        }

        private void DamageReceive(DamageEvent e)
        {
            var blood = Instantiate<ParticleSystem>(_bloodVFX);
            blood.transform.forward = e.normal;
            blood.transform.position = e.point + e.normal * 0.05f;
            blood.Play();
            
            if (!dead)
            {
                health.Remove(e.damage.amount); 

                if (health.value == 0)
                {
                    Die();
                }
            }
            
            if (dead)
            {
                if (e.source is Gun gun)
                {
                    var force = (e.point - gun.transform.position).normalized * impulseDamageScale;
                    ai.personTyped.rigid.AddForceAtPosition(force, e.point, ForceMode.Impulse);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            
            if (ai == null) ai = GetComponent<ZombieAI>();
            if (health == null) health = GetComponent<HealthManager>();
        }

        private void Start()
        {
            dead = false;
            
            health.FillValue();

            onDamageReceived.AddListener(DamageReceive);
        }
    }
}