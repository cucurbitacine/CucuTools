using System;
using CucuTools.Langs;
using UnityEngine;

namespace Example.Langs
{
    public class ExampleLangs : ExampleBlock
    {
        private const string LangIndexPrefs = nameof(LangIndexPrefs);
        
        private readonly static LangType[] Langs = (LangType[])Enum.GetValues(typeof(LangType));
        
        public override void ShowGUI()
        {
            GUILayout.Box("Change language:");
            
            foreach (var lang in Langs)
            {
                if (GUILayout.Button(lang.ToString()))
                {
                    CucuLangManager.CurrentLang = lang;
                    
                    PlayerPrefs.SetInt(LangIndexPrefs, (int)lang);
                }
            }
        }

        public override void Show()
        {
            base.Show();
            
            if (PlayerPrefs.HasKey(LangIndexPrefs))
            {
                CucuLangManager.CurrentLang = (LangType)(PlayerPrefs.GetInt(LangIndexPrefs) % Langs.Length);
            }
        }
    }
}