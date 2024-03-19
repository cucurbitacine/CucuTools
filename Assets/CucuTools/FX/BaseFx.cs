using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.FX
{
    public abstract class BaseFx : CucuBehaviour
    {
        public abstract bool isPlaying { get; }

        [DrawButton(group: "FX", colorHex: "aaaaff")]
        public abstract void Play();

        [DrawButton(group: "FX", colorHex: "ffaaaa")]
        public abstract void Stop();
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