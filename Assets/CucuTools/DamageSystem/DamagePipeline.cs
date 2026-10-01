using UnityEngine;

namespace CucuTools.DamageSystem
{
    public static class DamagePipeline
    {
        public static bool Process(DamageAgent sender, DamageAgent receiver, DamageData damage, DamageAttackHandler attackHandler, DamageHitHandler hitHandler)
        {
            if (receiver == null) return false;
            if (damage == null) return false;

            if (!receiver.AllowHitSelf && receiver == sender)
            {
                return false;
            }
            
            var process = DamageProcess.Get(sender, receiver, damage);

            try
            {
                if (attackHandler != null)
                {
                    attackHandler.Modify(process);
                }

                if (sender != null && sender.AttackHandler != null)
                {
                    sender.AttackHandler.Modify(process);
                }

                if (hitHandler != null)
                {
                    hitHandler.Modify(process);
                }

                if (receiver.HitHandler != null)
                {
                    receiver.HitHandler.Modify(process);
                }

                var damageEvent = new DamageEvent(process);

                receiver.OnDamageReceived(damageEvent);

                if (sender != null)
                {
                    sender.OnDamageSent(damageEvent);
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                DamageProcess.Release(process);
            }
            
            return true;
        }

        public static bool Process(DamageAgent sender, DamageAgent receiver, DamageData damage, DamageAttackHandler attackHandler)
        {
            return Process(sender, receiver, damage, attackHandler, null);
        }

        public static bool Process(DamageAgent sender, DamageAgent receiver, DamageData damage, DamageHitHandler hitHandler)
        {
            return Process(sender, receiver, damage, null, hitHandler);
        }

        public static bool Process(DamageAgent sender, DamageAgent receiver, DamageData damage)
        {
            return Process(sender, receiver, damage, null, null);
        }
        
        public static bool Process(DamageAgent receiver, DamageData damage, DamageHitHandler hitHandler)
        {
            return Process(null, receiver, damage, null, hitHandler);
        }

        public static bool Process(DamageAgent receiver, DamageData damage)
        {
            return Process(null, receiver, damage, null, null);
        }
        
        public static bool Process(AttackCollider attack, HitBox hitBox, DamageData damage)
        {
            return Process(attack.Owner, hitBox.Owner, damage, attack.AttackHandler, hitBox.HitHandler);
        }
        
        public static bool AttackHitboxAtPoint(AttackCollider attack, HitBox hitBox, Vector3 point, Vector3 normal)
        {
            if (!CanAttackHitbox(attack, hitBox)) return false;
            
            var damage = attack.BuildDamage(hitBox, point, normal);
            if (!Process(attack, hitBox, damage)) return false;
            
            attack.OnHit(hitBox.Owner);
            return true;
        }

        public static bool AttackTargetAtPoint(AttackCollider attack, Component target, Vector3 point, Vector3 normal)
        {
            return HitBox.Lookup(target, out var hitBox) &&
                   AttackHitboxAtPoint(attack, hitBox, point, normal);
        }

        public static bool AttackTarget(AttackCollider attack, Component target)
        {
            return AttackTargetAtPoint(attack, target, target.transform.position, target.transform.up);
        }
        
        public static bool CanAttackHitbox(AttackCollider attack, HitBox hitBox)
        {
            return IsValidHitboxForAttack(attack, hitBox) &&
                   CanAttackTarget(attack, hitBox.Owner);
        }
        
        public static bool IsValidHitboxForAttack(AttackCollider attack, HitBox hitBox)
        {
            if (IsNull(hitBox))
            {
                Debug.LogError($"{nameof(HitBox)} is Null");
                return false;
            }
            
            if (hitBox.Owner == null)
            {
                Debug.LogError($"{nameof(HitBox)} does not have {nameof(DamageAgent)}");
                return false;
            }

            var attackOwner = attack.Owner;
            if (attackOwner == null) return true;
            if (attackOwner.AllowHitSelf && attack.AllowHitSelf) return true;
            return attackOwner != hitBox.Owner;
        }
        
        public static bool CanAttackTarget(AttackCollider attackCollider, DamageAgent target)
        {
            if (target == null)
            {
                Debug.LogError($"{nameof(DamageAgent)} is null!");
                return false;
            }
            
            if (attackCollider.TargetCooldown <= 0f) return true;
            
            return attackCollider.GetCooldown(target) <= 0f;
        }
        
        private static bool IsNull(HitBox hitBox)
        {
            return hitBox == null || hitBox.Equals(null);
        }
    }

    public static class DamagePipelineExtenstion
    {
        public static bool SendDamage(this DamageAgent sender, DamageAgent receiver, DamageData damage)
        {
            return DamagePipeline.Process(sender, receiver, damage);
        }
        
        public static bool ReceiveDamage(this DamageAgent receiver, DamageAgent sender, DamageData damage)
        {
            return DamagePipeline.Process(sender, receiver, damage);
        }
        
        public static bool SendDamage(this DamageAgent sender, DamageAgent receiver, int damageAmount)
        {
            return sender.SendDamage(receiver, new DamageData(damageAmount));
        }
        
        public static bool ReceiveDamage(this DamageAgent receiver, DamageAgent sender, int damageAmount)
        {
            return receiver.ReceiveDamage(sender, new DamageData(damageAmount));
        }
    }
}