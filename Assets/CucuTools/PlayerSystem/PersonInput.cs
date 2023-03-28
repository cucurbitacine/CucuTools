namespace CucuTools.PlayerSystem
{
    public abstract class PersonInput : CucuBehaviour
    {
        public abstract PersonController Person { get; }
    }

    public abstract class PersonInput<TPerson> : PersonInput where TPerson : PersonController
    {
        public TPerson personTyped;

        public sealed override PersonController Person => personTyped;
    }
}