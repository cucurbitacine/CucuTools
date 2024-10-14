using System;

namespace CucuTools
{
    public interface IExecutable
    {
        public event Action<bool> ExecutionUpdated;
     
        public bool IsRunning { get; }
        
        public void Enter();
        public void Execute();
        public void Exit();
    }
}