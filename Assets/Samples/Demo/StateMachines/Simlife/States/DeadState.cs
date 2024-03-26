using CucuTools.StateMachines;

namespace Samples.Demo.StateMachines.Simlife.States
{
    public class DeadState : StateBase<CellController>
    {
        protected override void OnEnter()
        {
            //Destroy(core.gameObject);
            core.gameObject.SetActive(false);
        }
    }
}