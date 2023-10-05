using UnityEngine;

namespace CucuTools.FX.Impl
{
    public class ParticleSystemFx : BaseFx
    {
        [SerializeField] private ParticleSystem _particle;

        public ParticleSystem particle
        {
            get => _particle;
            set => _particle = value;
        }

        public override bool isPlaying => particle != null && particle.isPlaying;

        public override void Play()
        {
            if (particle)
            {
                particle.Stop();
                particle.Play();
            }
        }

        public override void Pause()
        {
            if (particle)
            {
                if (!particle.isPaused) particle.Pause();
            }
        }

        public override void UnPause()
        {
            if (particle)
            {
                if (particle.isPaused) particle.Play();
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