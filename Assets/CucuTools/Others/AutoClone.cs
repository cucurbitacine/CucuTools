namespace CucuTools.Others
{
    public sealed class AutoClone : CloneBehaviour
    {
        public bool autoFree = true;

        private float timer;
        
        private void OnEnable()
        {
            if (autoFree)
            {
                Free(false);
            }
        }

        private void OnDisable()
        {
            if (autoFree)
            {
                Free(true);
            }
        }
    }
}