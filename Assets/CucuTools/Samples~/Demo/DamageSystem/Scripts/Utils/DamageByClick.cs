using System;
using CucuTools.DamageSystem;
using Samples.Demo.Scripts.UI;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts.Utils
{
    public class DamageByClick : MonoBehaviour
    {
        [SerializeField] private DamageSource source;
        [SerializeField] private DamageReceiver receiver;
        [SerializeField] private SpriteButton button;

        private void HandleClick()
        {
            if (source && receiver)
            {
                source.SendDamage(source.CreateDamage(receiver), receiver);
            }
        }
        
        private void OnEnable()
        {
            if (button)
            {
                button.onClicked.AddListener(HandleClick);
            }
        }

        private void OnDisable()
        {
            if (button)
            {
                button.onClicked.RemoveListener(HandleClick);
            }
        }
    }
}
