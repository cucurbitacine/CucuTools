using System;
using UnityEngine;

namespace Samples.Demo.Scripts.UI
{
    public class TabController : MonoBehaviour
    {
        public int selected;
        public bool autoLink = false;
        [Space] public TabSelector[] tabs;

        public void Select(int value)
        {
            selected = value;

            UpdateTab();
        }

        public void UpdateTab()
        {
            for (var i = 0; i < tabs.Length; i++)
            {
                var tab = tabs[i];
                if (tab)
                {
                    if (autoLink)
                    {
                        tab.index = i;
                    }
                    
                    tab.UpdateTab(tab.index == selected);
                }
            }
        }

        private void OnEnable()
        {
            if (autoLink)
            {
                foreach (var tab in tabs)
                {
                    if (tab)
                    {
                        tab.onSelected.AddListener(Select);
                    }
                }
            }
        }

        private void OnDisable()
        {
            if (autoLink)
            {
                foreach (var tab in tabs)
                {
                    if (tab)
                    {
                        tab.onSelected.RemoveListener(Select);
                    }
                }
            }
        }

        private void Start()
        {
            UpdateTab();
        }

        private void OnValidate()
        {
            selected = tabs != null ? Mathf.Clamp(selected, 0, tabs.Length - 1) : 0;
        }
    }
}