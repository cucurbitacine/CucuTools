using UnityEngine;

namespace CucuTools.FX
{
    public class TestFx : CucuBehaviour
    {
        public float radius = 4f;
        public BaseFx prefab;
        public FxManager manager;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var fx = manager.GetOrCreate(prefab);

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var position = ray.GetPoint(10);
                var rotation = Quaternion.LookRotation(ray.direction, Vector3.up);
                fx.Play(position, rotation);
            }
        }
    }
}