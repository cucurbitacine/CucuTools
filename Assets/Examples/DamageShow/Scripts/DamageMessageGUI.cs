using System.Collections;
using System.Collections.Generic;
using CucuTools.DamageSystem;
using UnityEngine;

namespace Examples.DamageShow.Scripts
{
    public class DamageMessageGUI : MonoBehaviour
    {
        [Space] public float messageLife = 5f;

        private readonly List<MsgLife> _messages = new List<MsgLife>();

        public void LogMessage(string msg)
        {
            Debug.Log(msg);

            _messages.Add(new MsgLife() { msg = msg, life = 0f });
        }
        
        public void LogDamage(DamageInfo info)
        {
            var receiverName = info.receiver.manager != null ? $"{info.receiver.manager.name} to " : "";
            receiverName = $"{receiverName}{info.receiver.name}";

            var damageText =
                $"{info.damage.amount} of{(info.damage.critical ? " CRITICAL " : " ")}{info.damage.GetType().Name}";

            var sourceName = info.source.manager != null ? $"{info.source.manager.name} from " : "";
            sourceName = $"{sourceName}{info.source.name}";

            var logMessage = $"[{receiverName}] get [{damageText}] by [{sourceName}]";

            LogMessage(logMessage);
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

        private void Start()
        {
            StartCoroutine(_HandleMessages());

            foreach (var dmgMng in FindObjectsOfType<DamageManager>())
            {
                dmgMng.onDamageReceived.AddListener(LogDamage);
            }
        }

        private void OnGUI()
        {
            GUILayout.Space(128);

            for (var i = _messages.Count - 1; i >= 0; i--)
            {
                var msg = _messages[i];
                GUILayout.Box(msg.msg);
            }
        }

        private class MsgLife
        {
            public string msg;
            public float life;
        }
    }
}