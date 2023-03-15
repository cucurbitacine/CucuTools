using CucuTools.Serialization;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    [CustomEditor(typeof(SerializableGameObject))]
    public class SerializableGameObjectEditor : UnityEditor.Editor
    {
        private Vector2 scrollView;
        private bool showedList = true;
        
        public override void OnInspectorGUI()
        {
            var serializable = (SerializableGameObject)target;
            serializable.UpdateComponents();

            ShowTarget(serializable);
                
            GUILayout.Space(8);
            
            if (GUILayout.Button(showedList ? "Hide" : "Serializable Components"))
            {
                showedList = !showedList;
            }
            
            GUILayout.Space(8);
            
            if (showedList) ShowComponents(serializable.References.ToArray());
        }

        private void ShowTarget(SerializableGameObject entity)
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            
            GUILayout.FlexibleSpace();
            GUILayout.Label(entity.Cuid?.Guid.ToString() ?? "Without cuid");
            
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
        }
        
        private void ShowComponents(params ComponentReference[] components)
        {
            scrollView = GUILayout.BeginScrollView(scrollView);
            
            foreach (var component in components)
                ShowComponent(component);

            GUILayout.EndScrollView();
        }

        private void ShowComponent(ComponentReference component)
        {
            GUILayout.BeginHorizontal();

            component.IsEnabled = GUILayout.Toggle(component.IsEnabled, component.ComponentType.Name);
            EditorGUILayout.ObjectField(component.Component, component.ComponentType, true);

            GUILayout.EndHorizontal();
        }

        public const string MenuItem = Cucu.GameObject + Cucu.Serialization + Serialize; 
        public const string Serialize = "Serialize GameObject"; 
        
        [MenuItem(MenuItem, false, 10)]
        public static void SerializeGameObject(MenuCommand menuCommand)
        {
            GameObject target = menuCommand.context as GameObject;

            if (target == null) return;
            
            var cuid = CucuIdentity.GetOrAdd(target);
            
            if (target.GetComponent<SerializableGameObject>() == null)
                target.AddComponent<SerializableGameObject>();
/*
            if (target.GetComponent<SerializableTransform>() == null)
                target.AddComponent<SerializableTransform>();

            if (target.GetComponent<Rigidbody>() != null && target.GetComponent<SerializableRigidbody>() == null)
                target.AddComponent<SerializableRigidbody>();
            */
            Selection.activeObject = target; 
        }
        
        [MenuItem(MenuItem, true)]
        public static bool ValidateSerializeGameObject()
        {
            return Selection.activeGameObject != null;
        }
    }
}