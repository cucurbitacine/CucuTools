using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CucuTools.Attributes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CucuTools.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CucuBehaviour), true, isFallback = false)]
    public class CucuBehaviourEditor : UnityEditor.Editor
    {
        private const string DefaultButtonsGroupName = "Methods";

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DrawButtons(targets);
        }

        #region Drawing

        public static void DrawButtons(params Object[] targets)
        {
            var buttons = GetButtons(targets);
            
            if (buttons == null || !buttons.Any()) return;

            var grouped = buttons.GroupBy(b => b.attribute.Group).OrderBy(g => g.Min(gr => gr.attribute.Order));

            foreach (var group in grouped)
            {
                var groupName = string.IsNullOrEmpty(group.Key) ? DefaultButtonsGroupName : group.Key;
                DrawGroup(group, groupName, targets);
            }
        }

        private static void DrawGroup(IEnumerable<ButtonInfo> buttons, string groupName, params Object[] targets)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(groupName, GetHeaderGUIStyle());

            foreach (var button in buttons.OrderBy(b => b.attribute.Order))
                DrawButton(button, targets);
        } 
        
        private static void DrawButton(ButtonInfo button, params Object[] targets)
        {
            var attribute = button.attribute;
            var method = button.method;
                
            var buttonName = string.IsNullOrEmpty(attribute.Name) ? method.Name : attribute.Name;
            var backgroundColor = GUI.backgroundColor;

            try
            {
                if (!string.IsNullOrEmpty(button.attribute.ColorHex) &&
                    ColorUtility.TryParseHtmlString(button.attribute.ColorHex, out var color))
                    GUI.backgroundColor = color;

                if (GUILayout.Button(buttonName, GetButtonGUIStyle()))
                {
                    foreach (var target in targets)
                    {
                        method.Invoke(target, null);                        
                    }
                }
            }
            finally
            {
                GUI.backgroundColor = backgroundColor;
            }
        }

        #endregion

        #region Getter

        private static IEnumerable<MethodInfo> GetButtonMethods(params Object[] targets)
        {
            return targets.SelectMany(t => t.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(m => m.GetCustomAttributes(typeof(CucuButtonAttribute), true).Length > 0));
        }
        
        private static IEnumerable<ButtonInfo> GetButtons(params Object[] targets)
        {
            var methods = GetButtonMethods(targets)
                .Where(m => m.GetParameters().All(p => p.IsOptional));

            return methods.Select(m => new ButtonInfo(m)).Where(b => b.attribute != null);
        }

        #endregion

        #region GUIStyle

        private static GUIStyle GetButtonGUIStyle()
        {
            return new GUIStyle(GUI.skin.button);;
        }
        
        private static GUIStyle GetHeaderGUIStyle()
        {
            return new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                fontStyle = FontStyle.Bold, alignment = TextAnchor.UpperCenter
            };
        }

        #endregion
        
        public class ButtonInfo
        {
            public CucuButtonAttribute attribute;
            public MethodInfo method;

            public ButtonInfo(MethodInfo method)
            {
                this.method = method;

                attribute = (CucuButtonAttribute) this.method.GetCustomAttributes(typeof(CucuButtonAttribute), true)
                    .SingleOrDefault();
            }
        }
    }
}