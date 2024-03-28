using System.Reflection;
using SkalluUtils.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Editor.CustomEditors
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourEditor : UnityEditor.Editor
    {
        private bool _methodsShown;
        private MethodButtonAttributeHandler.CustomMethodInfo[] _methodsToShow;

        protected override void OnHeaderGUI()
        {
            EditorGUILayout.LabelField("Header");
            
            DrawHeader();
        }

        public override void OnInspectorGUI()
        {
            MethodButtonAttributeHandler.DrawMethods(target, ref _methodsToShow, ref _methodsShown);

            //DrawInspectorFields();
            
            EditorGUILayout.LabelField("Body");
        }

        private void DrawInspectorFields()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.UpdateIfRequiredOrScript();
            
            TooltipAttribute tooltipAttribute = serializedObject.targetObject.GetType().GetCustomAttribute<TooltipAttribute>();
            bool showTooltip = tooltipAttribute != null;
            
            SerializedProperty iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                if (iterator.propertyPath.Equals("m_Script"))
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        EditorGUILayout.PropertyField(iterator, new GUIContent("Script",
                            showTooltip ? $"{tooltipAttribute.tooltip}" : null), true);
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(iterator, true);
                }
            }
            
            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }
    }
}