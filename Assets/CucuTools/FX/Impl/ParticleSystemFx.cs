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

        public override void Play()
        {
            if (particle)
            {
                particle.Stop();
                particle.Play();
            }
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