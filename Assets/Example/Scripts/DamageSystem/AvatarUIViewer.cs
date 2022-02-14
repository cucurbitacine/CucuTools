using System;
using System.Collections;
using CucuTools.Avatar;
using UnityEngine;
using UnityEngine.UI;

namespace Example.Scripts.DamageSystem
{
    public class AvatarUIViewer : MonoBehaviour
    {
        [Min(0.01f)]
        public float Speed = 100;
        
        [Space]
        public Image HealthImage;
        public HealthBehaviour Health;

        [Space]
        public Image StaminaImage;

        private Coroutine _updatingHealth;
        private Coroutine _updatingStamina;


        public void UpdateHealthView()
        {
            if (_updatingHealth != null) StopCoroutine(_updatingHealth);
            _updatingHealth = StartCoroutine(UpdatingHealth());
        }

        private IEnumerator UpdatingHealth()
        {
            var start = HealthImage.fillAmount;
            var target = (float) Health.Amount / Health.Maximum;
            
            var distance = Mathf.Abs(start - target);
            var duration = distance / Speed;
            
            var timer = 0f;
            while (timer < duration)
            {
                var t = Mathf.SmoothStep(0f, 1f, timer / duration);
                HealthImage.fillAmount = Mathf.Lerp(start, target, t);
                timer += Time.deltaTime;
                yield return null;
            }

            HealthImage.fillAmount = target;
        }

        private void Start()
        {
            UpdateHealthView();

            Health.OnDied.AddListener(() => Health.SetHealth(Health.Maximum));
        }

        public CucuAvatar Avatar;
        public float Stamina = 100;
        public float StaminaMax = 100;
        public float StaminaCost = 25;
        public float StaminaRestore = 10;
        
        private void Update()
        {
            if (Avatar.Brain.InputInfo.sprint)
            {
                Stamina -= StaminaCost * Time.deltaTime;
            }
            else
            {
                Stamina += StaminaRestore * Time.deltaTime;
            }

            Stamina = Mathf.Clamp(Stamina, 0f, StaminaMax);

            StaminaImage.fillAmount = Mathf.Lerp(StaminaImage.fillAmount, Stamina / StaminaMax, 8 * Time.deltaTime);
        }
    }
}
