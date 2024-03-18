using System.Collections.Generic;
using System.Linq;

namespace CucuTools.FX.Impl
{
    public class GroupFx : BaseFx
    {
        public List<BaseFx> fx = new List<BaseFx>();

        public override bool isPlaying => fx.Any(f => f.isPlaying);

        public override void Play()
        {
            for (var i = 0; i < fx.Count; i++)
            {
                fx[i]?.Play();
            }
        }

        public override void Pause()
        {
            for (var i = 0; i < fx.Count; i++)
            {
                fx[i]?.Pause();
            }
        }

        public override void Unpause()
        {
            for (var i = 0; i < fx.Count; i++)
            {
                fx[i]?.Unpause();
            }
        }

        public override void Stop()
        {
            for (var i = 0; i < fx.Count; i++)
            {
                fx[i]?.Stop();
            }
        }
    }
}