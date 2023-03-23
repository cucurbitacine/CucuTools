using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace CucuTools.MultiLangSystem.Impl
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class MultiLangImageUI : MultiLangBehaviour
    {
        private Image imageUI;

        #region SerializeField

        [CucuReadOnly]
        [SerializeField] private bool isValid;
        [SerializeField] private ContentSource<Sprite> contentSource;

        #endregion

        #region Properties

        public ContentSource<Sprite> ContentSource => contentSource;
        public Image ImageUI => imageUI != null ? imageUI : (imageUI = GetComponent<Image>());
        
        public bool IsValid
        {
            get => isValid;
            private set => isValid = value;
        }

        #endregion

        public override void UpdateLang()
        {
            if (IsValid) ImageUI.sprite = ContentSource.GetContent();
        }

        private void Validate()
        {
            IsValid = ImageUI != null && ContentSource != null;
        }

        private void Start()
        {
            Validate();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            
            Validate();
        }
    }
}