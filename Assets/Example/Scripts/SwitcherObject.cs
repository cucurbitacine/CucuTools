using CucuTools;
using CucuTools.Attributes;

namespace Example.Scripts
{
    public class SwitcherObject : CucuBehaviour
    {
        public bool activeDefault = true;
        public bool switchOnStart = true;
    
        public void Switch(bool value)
        {
            gameObject.SetActive(value);
        }
        
        [CucuButton()]
        public void On()
        {
            Switch(true);
        }

        [CucuButton()]
        public void Off()
        {
            Switch(false);
        }

        private void Start()
        {
            if (switchOnStart) Switch(activeDefault);
        }
    }
}