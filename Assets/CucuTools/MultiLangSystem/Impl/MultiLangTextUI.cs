using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace CucuTools.MultiLangSystem.Impl
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class MultiLangTextUI : MultiLangBehaviour
    {
        private Text textUI;

        #region SerializeField

        [CucuReadOnly]
        [SerializeField] private string textDisplay;
        
        [CucuReadOnly]
        [SerializeField] private bool isValid;
        [SerializeField] private ContentSource<string> contentSource;

        #endregion

        #region Properties

        public ContentSource<string> ContentSource => contentSource;
        public Text TextUI => textUI != null ? textUI : (textUI = GetComponent<Text>());
        
        public bool IsValid
        {
            get => isValid;
            private set => isValid = value;
        }

        #endregion

        public override void UpdateLang()
        {
            if (IsValid) TextUI.text = ContentSource.GetContent();
            
            textDisplay = TextUI?.text ?? string.Empty;
        }

        private void Validate()
        {
            IsValid = TextUI != null && ContentSource != null;
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