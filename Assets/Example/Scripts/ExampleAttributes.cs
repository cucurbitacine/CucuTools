using CucuTools;
using CucuTools.Attributes;
using UnityEngine;

namespace Example.Scripts
{
    public class ExampleAttributes : CucuBehaviour
    {
        [CucuScene]
        public string sceneName;

        [Space]
        
        [CucuReadOnly]
        public bool boolValue = true;
        [CucuReadOnly]
        public int intValue = 42;
        [CucuReadOnly]
        public float floatValue = 1.618f;
        [CucuReadOnly]
        public string stringValue = "Hello world";
        
        [Space]
        
        [CucuLayer]
        public int intLayer;
         
        [CucuButton]
        public void SomeMethod1()
        {
            Debug.Log("1");
        }
        
        [CucuButton("Some Method 2", 1, "Grouped")]
        public void SomeMethod2()
        {
            Debug.Log("2");
        }
        
        [CucuButton(name: "Some Method 3", order: 0, group: "Grouped", colorHex: "ff0000")]
        public void SomeMethod3()
        {
            Debug.Log("3");
        }
    }
}