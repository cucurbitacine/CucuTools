using System.Collections;
using CucuTools.FX;
using CucuTools.Pools;
using UnityEngine;

namespace CucuTools.Others
{
    public sealed class PoolObjectFx : PoolObject
    {
        public BaseFx fx;

        private Coroutine _releasing;

        private IEnumerator Releasing()
        {
            while (fx.isPlaying)
            {
                yield return null;
            }
            
            Release();
        }
        
        private void PlayStarted(BaseFx t)
        {
            if (_releasing != null) StopCoroutine(_releasing);
            _releasing = StartCoroutine(Releasing());
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            if (fx == null) fx = GetComponent<BaseFx>();
        }

        private void OnEnable()
        {
            if (fx)
            {
                fx.onPlayStarted.AddListener(PlayStarted);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (_releasing != null) StopCoroutine(_releasing);
            
            if (fx)
            {
                fx.onPlayStarted.RemoveListener(PlayStarted);
            }
        }

        private void OnValidate()
        {
            if (fx == null) fx = GetComponent<BaseFx>();
        }
    }
}