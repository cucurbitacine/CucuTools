using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.FX
{
    public abstract class BaseFx : CucuBehaviour
    {
        public bool playOnStart = false;
        [Space]
        [SerializeField] private UnityEvent<BaseFx> _onPlayStarted = new UnityEvent<BaseFx>();
        
        public abstract bool isPlaying { get; }

        public UnityEvent<BaseFx> onPlayStarted => _onPlayStarted;
        
        [DrawButton(group: "FX", colorHex: "aaaaff")]
        public void Play()
        {
            if (PlayInternal())
            {
                onPlayStarted.Invoke(this);
            }
        }

        [DrawButton(group: "FX", colorHex: "ffaaaa")]
        public abstract void Stop();

        protected abstract bool PlayInternal();
        
        protected virtual void Start()
        {
            if (playOnStart)
            {
                Play();
            }
        }
    }

    public static class BaseFxExt
    {
        public static void Play(this BaseFx fx, Vector3 position, Quaternion rotation)
        {
            fx.transform.SetPositionAndRotation(position, rotation);
            fx.Play();
        }

        public static void Play(this BaseFx fx, Vector3 position)
        {
            fx.Play(position, fx.transform.rotation);
        }

        public static void Play(this BaseFx fx, Quaternion rotation)
        {
            fx.Play(fx.transform.position, rotation);
        }
    }
}