using System;
using System.Collections.Generic;

namespace CucuTools.Interactables
{
    public class InteractHandler
    {
        public ICucuContext Context { get; private set; }
        public bool Pressing { get; private set; }

        public IReadOnlyList<ICucuInteractable> Currents => currents;
        public IReadOnlyList<ICucuInteractable> Previous => previous;

        private readonly List<ICucuInteractable> currents = new List<ICucuInteractable>();
        private readonly List<ICucuInteractable> previous = new List<ICucuInteractable>();

        private readonly Dictionary<ICucuInteractable, bool> canPressing = new Dictionary<ICucuInteractable, bool>();
        private readonly Dictionary<ICucuInteractable, bool> wasPressing = new Dictionary<ICucuInteractable, bool>();

        private readonly ICucuInteractable[] emptyArray = Array.Empty<ICucuInteractable>();
        
        public void Update(ICucuContext context, bool pressing, params ICucuInteractable[] interactables)
        {
            Context = context;
            Pressing = pressing;

            previous.Clear();
            previous.AddRange(currents);

            currents.Clear();
            currents.AddRange(interactables ?? emptyArray);

            previous.ForEach(UpdatePrevious);
            currents.ForEach(UpdateCurrents);
        }

        private void UpdateCurrents(ICucuInteractable current)
        {
            if (!wasPressing.ContainsKey(current)) wasPressing[current] = false;
            canPressing[current] = current.InteractInfo.Press || (current.InteractInfo.Hover && !wasPressing[current]);

            if (canPressing[current] && Pressing) current.Press(Context);
            else current.Hover(Context);

            wasPressing[current] = Pressing;
        }

        private void UpdatePrevious(ICucuInteractable prev)
        {
            if (!currents.Contains(prev)) prev.Idle();
        }
    }
}