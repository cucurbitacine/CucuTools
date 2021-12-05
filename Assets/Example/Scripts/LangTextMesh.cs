using System;
using CucuTools.Attributes;
using CucuTools.Langs;
using CucuTools.Langs.Impl;
using UnityEngine;

namespace Example.Scripts
{
    [RequireComponent(typeof(TextMesh))]
    public class LangTextMesh : CucuLangBehaviour
    {
        private TextMesh _textMesh;

        public LangStringSource StringSource;

        public TextMesh TextMesh => _textMesh != null ? _textMesh : (_textMesh = GetComponent<TextMesh>());
        
        public override void UpdateLang()
        {
            if (StringSource != null) TextMesh.text = StringSource.Content;
        }

        [CucuButton("Update Text")]
        private void UpdateTextButton()
        {
            UpdateLang();
        }
    }
}
