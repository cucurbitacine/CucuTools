using CucuTools;
using CucuTools.FX;
using UnityEngine;

namespace Samples.Demo.FX
{
    public class TestFx : CucuBehaviour
    {
        public float radius = 4f;
        public BaseFx prefab;

        public PrefabManager prefabManager;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (prefabManager == null) prefabManager = Cucu.PrefabManager;
                
                var fx = prefabManager.Create(prefab.gameObject).GetComponent<BaseFx>();

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var position = ray.GetPoint(10);
                var rotation = Quaternion.LookRotation(ray.direction, Vector3.up);
                fx.Play(position, rotation);
            }
        }
    }
}