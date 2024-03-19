using CucuTools.Pools;

namespace CucuTools.FX.Impl
{
    public class PoolObjectFx : PoolObject
    {
        public BaseFx fx;

        private void Awake()
        {
            if (fx == null) fx = GetComponent<BaseFx>();
        }

        private void Update()
        {
            if (fx && !fx.isPlaying)
            {
                Release();
            }
        }

        private void OnValidate()
        {
            if (fx == null) fx = GetComponent<BaseFx>();
        }
    }
}