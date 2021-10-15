using CucuTools.Attributes;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(CucuReadOnlyAttribute))]
    public class CucuReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            string labelStr = $"{label.text} (read only)";
 
            switch (prop.propertyType)
            {
                case SerializedPropertyType.Integer:
                    EditorGUI.IntField(position, labelStr, prop.intValue);
                    break;
                case SerializedPropertyType.Boolean:
                    EditorGUI.Toggle(position, labelStr, prop.boolValue);
                    break;
                case SerializedPropertyType.Float:
                    EditorGUI.FloatField(position, labelStr, prop.floatValue);
                    break;
                case SerializedPropertyType.String:
                    EditorGUI.TextField(position, labelStr, prop.stringValue);
                    break;
                case SerializedPropertyType.Color:
                    EditorGUI.ColorField(position, labelStr, prop.colorValue);
                    break;
                default:
                    EditorGUI.LabelField(position, labelStr, "<not supported>");
                    break;
            }
        }
    }
}