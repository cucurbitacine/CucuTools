using CucuTools;
using UnityEngine;

namespace Example
{
    public class ExampleController : MonoBehaviour
    {
        private ExampleBlock Current;
        private ExampleBlock[] Examples;

        private void Awake()
        {
            Examples = FindObjectsOfType<ExampleBlock>(true);
        }

        private void OnGUI()
        {
            if (Current == null)
            {
                GUILayout.Box("Select Example", GUILayout.Width(512));
                
                foreach (var example in Examples)
                {
                    if (GUILayout.Button(example.BlockName, GUILayout.Width(256)))
                    {
                        Current = example;
                        Current.Show();
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Return", GUILayout.Width(128)))
                {
                    Current.Hide();
                    Current = null;
                }
                else Current.ShowGUI();
            }
        }
    }
    
    public abstract class ExampleBlock : CucuBehaviour
    {
        public virtual GameObject Scene => gameObject;
        
        public virtual string BlockName => name;

        public abstract void ShowGUI();

        public virtual void Show()
        {
            Scene.SetActive(true);
        }
        
        public virtual void Hide()
        {
            Scene.SetActive(false);
        }

        protected virtual void Awake()
        {
            Scene.SetActive(false);
        }
    }
}
