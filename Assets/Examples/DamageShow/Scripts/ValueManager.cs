using CucuTools;
using UnityEngine;
using UnityEngine.Events;

namespace Examples.DamageShow.Scripts
{
    public class ValueManager : CucuBehaviour
    {
        [Min(0)]
        public int value = 0;
        [Min(0)]
        public int valueMax = 0;

        public UnityEvent<int> onValueChanged = new UnityEvent<int>();

        public void Change(int delta)
        {
            value = Mathf.Clamp(value + delta, 0, valueMax);

            OnValueChanged();
            
            onValueChanged.Invoke(value);
        }

        public void Add(int add)
        {
            if (add > 0) Change(add);
        }

        public void Remove(int remove)
        {
            if (remove > 0) Change(-remove);
        }
        
        public void ClearValue()
        {
            Remove(valueMax);
        }
        
        public void FillValue()
        {
            Add(valueMax);
        }

        protected virtual void OnValueChanged()
        {
        }
        
        private void OnValidate()
        {
            value = Mathf.Clamp(value, 0, valueMax);
        }
    }
}