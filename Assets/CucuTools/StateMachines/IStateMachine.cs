namespace CucuTools.StateMachines
{
    public interface IStateMachine : IState
    {
        public IState SubState { get; }

        public void SetSubState(IState state);
    }
    
    public interface IState : IDone, IExecutable
    {
    }
}