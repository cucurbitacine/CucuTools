using System;
using System.Collections;
using CucuTools.DamageSystem;
using UnityEngine;

namespace Example.Scripts.DamageSystem.Balloon
{
    public class BalloonController : MonoBehaviour
    {
        private Vector3 _localScale = Vector3.one;
        private Coroutine _scaling = default;
        
        public DamageReceiver Receiver = default;
        public HealthBehaviour Health = default;
        public DamageEffect Effect = default;
        public BalloonHealthViewer Viewer = default;

        private void Appear()
        {
            Receiver.IsEnabled = true;
            
            if (_scaling != null) StopCoroutine(_scaling);
            _scaling = StartCoroutine(Scaling(Vector3.zero, _localScale));
        }

        private void Disappear()
        {
            Receiver.IsEnabled = false;
            
            if (_scaling != null) StopCoroutine(_scaling);
            _scaling = StartCoroutine(Scaling(_localScale, Vector3.zero, Destroy));
        }

        private void Destroy()
        {
            Instantiate(gameObject);
            Destroy(gameObject);
        }
        
        private IEnumerator Scaling(Vector3 start, Vector3 target, Action callback = null)
        {
            transform.localScale = start;
            
            var timer = 0f;
            var duration = 1f;
            while (timer < duration)
            {
                var t = Mathf.SmoothStep(0f, 1f, timer / duration);
                
                transform.localScale = Vector3.Lerp(start, target, t);
                
                timer += Time.deltaTime;
                yield return null;
            }

            transform.localScale = target;
            
            callback?.Invoke();
        }

        private void Awake()
        {
            //_localScale = transform.localScale;
        }

        private void OnEnable()
        {
            Appear();
        }

        private void Start()
        {
            Health.OnDied.AddListener(() => Receiver.IsEnabled = false);
            Health.OnDied.AddListener(Disappear);

            Health.Amount = Health.Maximum;
            Receiver.OnDamageReceived.AddListener(Health.ReceiveDamage);

            Effect.Health = Health;
            Health.OnDamageApplied.AddListener(Effect.ReceiveDamage);
            
            Viewer.Health = Health;
            Viewer.UpdateView();
            Health.OnHealthChanged.AddListener(_ => Viewer.UpdateView());
        }
    }
}