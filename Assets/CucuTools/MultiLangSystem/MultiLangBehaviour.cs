namespace CucuTools.MultiLangSystem
{
    public abstract class MultiLangBehaviour : CucuBehaviour
    {
        public abstract void UpdateLang();

        protected virtual void Awake()
        {
            MultiLangManager.AddListener(UpdateLang);
        }

        protected virtual void OnEnable()
        {
            UpdateLang();
        }

        protected virtual void OnValidate()
        {
            UpdateLang();
        }
    }
}