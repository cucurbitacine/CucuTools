using UnityEngine;
using UnityEngine.Events;

namespace Samples.Demo.Scripts.UI
{
    public class TabSelector : SpriteButton
    {
        [Header("Tab Selector")]
        public int index = 0;
        public GameObject tab;
        [Space]
        public UnityEvent<int> onSelected = new UnityEvent<int>();
        
        public void SelectTab()
        {
            onSelected.Invoke(index);
        }
        
        public void UpdateTab(bool select)
        {
            Select(select);

            if (tab)
            {
                tab.SetActive(select);
            }
        }
        
        public void ShowTab()
        {
            UpdateTab(true);
        }
        
        public void HideTab()
        {
            UpdateTab(false);
        }
        
        private void OnEnable()
        {
            onClicked.AddListener(SelectTab);
        }
        
        private void OnDisable()
        {
            onClicked.RemoveListener(SelectTab);
        }
    }
}