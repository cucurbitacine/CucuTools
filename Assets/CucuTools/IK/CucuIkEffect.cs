namespace CucuTools.IK
{
    public abstract class CucuIKEffect : CucuIKBehaviour
    {
        public abstract void OnSolved();
        
        protected virtual void LateUpdate()
        {
            if (Brain != null) OnSolved();
        }
    }
}