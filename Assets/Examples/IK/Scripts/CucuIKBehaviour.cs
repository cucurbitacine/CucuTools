using CucuTools;
using UnityEngine;

namespace Examples.IK.Scripts
{
    public abstract class CucuIKBehaviour : CucuBehaviour
    {
        [SerializeField] private CucuIKBrain brain = default;
        
        public CucuIKBrain Brain
        {
            get => brain;
            set => brain = value;
        }
    }
}