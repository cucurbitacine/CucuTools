using System;
using System.Collections;
using System.Collections.Generic;
using CucuTools.DamageSystem;
using CucuTools.PlayerSystem;
using UnityEngine;

namespace Examples.DamageSystem
{
    public class DamageSystemSceneController : MonoBehaviour
    {
        public DamageSource source;
        public PlayerRigidInput input;

        [Space]
        public LayerMask shootLayers = 1;
        
        [Space]
        public float messageLife = 5f;

        [Space]
        public float delay = 0.1f;
        public LineRenderer projectile;

        private Coroutine _delay = null;

        private readonly List<MsgLife> _messages = new List<MsgLife>();

        public void LogDamage(DamageInfo info)
        {
            var receiverName = info.receiver.manager != null ? $"{info.receiver.manager.name} to " : "";
            receiverName = $"{receiverName}{info.receiver.name}";

            var damageText =
                $"{info.damage.amount} of{(info.damage.critical ? " CRITICAL " : " ")}{info.damage.GetType().Name}";

            var sourceName = info.source.manager != null ? $"{info.source.manager.name} from " : "";
            sourceName = $"{sourceName}{info.source.name}";

            var logMessage = $"[{receiverName}] get [{damageText}] by [{sourceName}]";

            Debug.Log(logMessage);

            _messages.Add(new MsgLife() { msg = logMessage, life = 0f });
        }

        private void Projectile(Vector3 start, Vector3 end)
        {
            projectile.useWorldSpace = true;

            projectile.positionCount = 2;
            projectile.SetPosition(0, start);
            projectile.SetPosition(1, end);

            if (_delay != null) StopCoroutine(_delay);
            _delay = StartCoroutine(_Delay());
        }

        private IEnumerator _Delay()
        {
            projectile.enabled = true;

            yield return new WaitForSeconds(delay);

            projectile.enabled = false;
        }

        private IEnumerator _HandleMessages()
        {
            while (true)
            {
                yield return null;
                
                foreach (var message in _messages)
                {
                    message.life += Time.deltaTime;
                }

                _messages.RemoveAll(m => messageLife < m.life);
            }
        }

        private class MsgLife
        {
            public string msg;
            public float life;
        }
        
        private void Start()
        {
            StartCoroutine(_HandleMessages());
        }

        private void Update()
        {
            if (input.shoot)
            {
                var ray = new Ray(input.player.eyes.position, input.player.eyes.forward);
                if (Physics.Raycast(ray, out var hit, 100f, shootLayers))
                {
                    Projectile(source.transform.position, hit.point);
                    
                    var rcv = hit.collider.GetComponent<DamageReceiver>();
                    if (rcv)
                    {
                        var dmg = source.GenerateDamage(rcv);

                        dmg.point = hit.point;
                        dmg.normal = hit.normal;

                        rcv.ReceiveDamage(dmg);
                    }
                }
                else
                {
                    Projectile(source.transform.position, ray.origin + ray.direction * 100);
                }
            }
        }

        private void OnGUI()
        {
            GUILayout.Space(128);
            
            foreach (var msg in _messages)
            {
                GUILayout.Box(msg.msg);
            }
        }
    }
}