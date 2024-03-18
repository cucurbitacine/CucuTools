namespace CucuTools.FX.Impl
{
    public class CloneFx : CloneBehaviour
    {
        public BaseFx fx;

        private void Start()
        {
            if (fx == null) fx = GetComponent<BaseFx>();
        }

        private void Update()
        {
            if (fx)
            {
                Free(!fx.isPlaying);
            }
        }

        private void OnValidate()
        {
            if (fx == null) fx = GetComponent<BaseFx>();
        }
    }
}