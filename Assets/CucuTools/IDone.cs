using System;

namespace CucuTools
{
    public interface IDone
    {
        public event Action<bool> Done; 
        
        public bool IsDone { get; }
        
        public void SetDone(bool value);
    }
}