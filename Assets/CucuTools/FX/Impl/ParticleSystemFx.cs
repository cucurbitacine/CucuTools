using UnityEngine;

namespace CucuTools.FX.Impl
{
    public class ParticleSystemFx : VisualFx
    {
        [SerializeField] private ParticleSystem _particle;

        public ParticleSystem particle
        {
            get => _particle;
            set => _particle = value;
        }

        public override bool isPlaying => particle && particle.isPlaying;

        protected override bool PlayInternal()
        {
            if (particle)
            {
                particle.Stop();
                particle.Play();
                return true;
            }

            return false;
        }
        
        public override void Stop()
        {
            if (particle)
            {
                particle.Stop();
            }
        }
        
        private void Awake()
        {
            if (particle == null) particle = GetComponent<ParticleSystem>();
        }
    }
}