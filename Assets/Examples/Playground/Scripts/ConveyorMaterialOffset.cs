using CucuTools.PlayerSystem.Tools;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    public class ConveyorMaterialOffset : MonoBehaviour
    {
        [Space]
        public ConveyorRigid conveyor;
        public MaterialOffsetShift shift;

        private void Update()
        {
            var direction = new Vector2(conveyor.localDirection.x, conveyor.localDirection.z);
            var speed = conveyor.speed;
            
            var offset = direction * (speed * Time.time);

            shift.offset = offset;
        }
    }
}