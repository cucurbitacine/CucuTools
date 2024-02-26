using UnityEngine;

namespace Samples.Demo.Scripts.UI
{
    public class TabButton : SpriteButton
    {
        [Space]
        public GameObject tab;

        public void SelectTab(bool select)
        {
            Select(select);

            if (tab)
            {
                tab.SetActive(select);
            }
        }
        
        public void SelectTab()
        {
            SelectTab(true);
        }
        
        public void DeselectTab()
        {
            SelectTab(false);
        }
    }
}