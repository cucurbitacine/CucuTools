using System;

namespace CucuTools
{
    public interface IPausable
    {
        public event Action<bool> Paused; 
        
        public bool IsPaused { get; }
        
        public void Pause(bool value);
    }
}