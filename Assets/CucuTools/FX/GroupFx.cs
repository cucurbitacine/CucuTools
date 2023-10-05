using System;
using System.Linq;

namespace CucuTools.FX
{
    public class GroupFx : BaseFx
    {
        public BaseFx[] fx = Array.Empty<BaseFx>();

        public override bool isPlaying => fx.Any(f => f.isPlaying);

        public override void Play()
        {
            for (var i = 0; i < fx.Length; i++)
            {
                fx[i]?.Play();
            }
        }

        public override void Pause()
        {
            for (var i = 0; i < fx.Length; i++)
            {
                fx[i]?.Pause();
            }
        }

        public override void UnPause()
        {
            for (var i = 0; i < fx.Length; i++)
            {
                fx[i]?.UnPause();
            }
        }

        public override void Stop()
        {
            for (var i = 0; i < fx.Length; i++)
            {
                fx[i]?.Stop();
            }
        }
    }
}