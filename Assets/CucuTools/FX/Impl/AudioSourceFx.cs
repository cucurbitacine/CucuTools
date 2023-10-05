using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CucuTools.FX.Impl
{
    public class AudioSourceFx : AudioFx
    {
        [SerializeField] private AudioSource _source;
        
        [Space] public SelectMode mode = SelectMode.Current;
        [Min(-1)]
        public int index = -1;
        public AudioClip[] clips = Array.Empty<AudioClip>();

        public AudioSource source
        {
            get => _source;
            set => _source = value;
        }
        
        public override bool isPlaying => source != null && source.isPlaying;
        
        public override void Play()
        {
            if (source)
            {
                source.clip = GetClip();
                source.Play();
            }
        }

        public override void Pause()
        {
            if (source)
            {
                source.Pause();
            }
        }

        public override void UnPause()
        {
            if (source)
            {
                source.UnPause();
            }
        }

        public override void Stop()
        {
            if (source)
            {
                source.Stop();
            }
        }
        
        private AudioClip GetClip()
        {
            if (clips.Length == 0)
            {
                return source ? source.clip : null;
            }

            var selected = index;
            
            switch (mode)
            {
                case SelectMode.Current:
                    selected = index;
                    break;
                case SelectMode.Next:
                    selected = (index + 1) % clips.Length; 
                    break;
                case SelectMode.Random:
                    selected = Random.Range(0, clips.Length);
                    break;
            }
            
            index = Mathf.Clamp(selected, 0, clips.Length - 1);
                
            return clips[index];
        }

        private void Awake()
        {
            if (source == null) source = GetComponent<AudioSource>();
        }

        private void OnValidate()
        {
            if (clips.Length > 0)
            {
                index = Mathf.Clamp(index, 0, clips.Length - 1);
            }
            else
            {
                index = -1;
            }
        }

        public enum SelectMode
        {
            Current,
            Next,
            Random,
        }
    }
}