using System;
using System.Collections.Generic;
using System.Linq;
using CucuTools.Langs;
using UnityEngine;

namespace Example.Scripts
{
    public class ExampleSceneController : MonoBehaviour
    {
        private List<LangType> _langList;
        private int _langIndex = 0;
        private string[] _langNames;
        
        public LangType currentLang = LangType.Eng;
        
        private void Awake()
        {
            _langList = ((LangType[]) Enum.GetValues(typeof(LangType))).ToList();
            _langNames = _langList.Select(l => l.ToString()).ToArray();
        }

        private void Start()
        {
            _langIndex = _langList.IndexOf(currentLang);
            CucuLangManager.CurrentLang = currentLang;
            
            SwitchCursor(false);
        }

        private void SwitchCursor(bool value)
        {
            if (value)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = transform;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false; 
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _langIndex = (_langIndex + 1) % _langList.Count;
                currentLang = _langList[_langIndex];
                CucuLangManager.CurrentLang = currentLang;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                SwitchCursor(false);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SwitchCursor(true);
            }
        }

        private void OnGUI()
        {
            _langIndex = GUILayout.Toolbar(_langIndex, _langNames);
            currentLang = _langList[_langIndex];
            CucuLangManager.CurrentLang = currentLang;
        }
    }
}
