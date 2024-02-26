using UnityEngine;

namespace Samples.Demo.Scripts.UI
{
    public class TabController : MonoBehaviour
    {
        public int selected;
        [Space] public TabButton[] tabs;

        public void Select(int value)
        {
            selected = value;

            UpdateTab();
        }

        public void UpdateTab()
        {
            for (var i = 0; i < tabs.Length; i++)
            {
                if (tabs[i])
                {
                    tabs[i].SelectTab(selected == i);
                }
            }
        }

        private void Start()
        {
            UpdateTab();
        }
    }
}