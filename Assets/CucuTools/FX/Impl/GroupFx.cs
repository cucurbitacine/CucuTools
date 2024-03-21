using System.Collections.Generic;
using System.Linq;

namespace CucuTools.FX.Impl
{
    public class GroupFx : BaseFx
    {
        public List<BaseFx> fx = new List<BaseFx>();

        public override bool isPlaying => fx.Any(f => f.isPlaying);

        protected override bool PlayInternal()
        {
            for (var i = 0; i < fx.Count; i++)
            {
                fx[i]?.Play();
            }

            return true;
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