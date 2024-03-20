using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CucuTools.FX.Impl
{
    public class AudioSourceFx : AudioFx
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource _source;
        public bool loop = false;
        
        [Header("Audio Clips")]
        public SelectMode selectMode = SelectMode.Current;
        [Min(-1)]
        public int selected = -1;
        public List<AudioClip> clips = new List<AudioClip>();

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

        public override void Stop()
        {
            if (source)
            {
                source.Stop();
            }
        }
        
        private AudioClip GetClip()
        {
            if (clips.Count == 0)
            {
                return source ? source.clip : null;
            }

            var next = selected;
            
            switch (selectMode)
            {
                case SelectMode.Current:
                    next = selected;
                    break;
                case SelectMode.Next:
                    next = (selected + 1) % clips.Count; 
                    break;
                case SelectMode.Random:
                    next = Random.Range(0, clips.Count);
                    break;
            }
            
            selected = Mathf.Clamp(next, 0, clips.Count - 1);
                
            return clips[selected];
        }

        private void Awake()
        {
            if (source == null) source = GetComponent<AudioSource>();

            if (source)
            {
                source.playOnAwake = false;
                source.loop = loop;
            }
        }

        private void OnValidate()
        {
            if (clips.Count > 0)
            {
                selected = Mathf.Clamp(selected, 0, clips.Count - 1);
            }
            else
            {
                selected = -1;
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