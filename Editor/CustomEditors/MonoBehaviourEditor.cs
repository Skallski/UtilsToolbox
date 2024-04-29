using System.Linq;
using System.Reflection;
using SkalluUtils.Editor.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SkalluUtils.Editor.CustomEditors
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourEditor : UnityEditor.Editor
    {
        private MethodButtonAttributeHandler.CustomMethodInfo[] _methodsToShow;
        private GUIStyle _iconStyle;
        
        private void OnEnable()
        {
            EditorHeaderGuiInitializer.OnDrawHeaderGUIEvent += OnHeaderItemGUI;
            EditorHeaderGuiInitializer.InitializeHeaderGUI(target as MonoBehaviour);
        }

        private void OnDisable()
        {
            EditorHeaderGuiInitializer.OnDrawHeaderGUIEvent -= OnHeaderItemGUI;
        }

        protected virtual void OnHeaderItemGUI(Rect rect, Object targetObject)
        {
            DrawEditButton(rect, targetObject);
            rect.x -= EditorHeaderGuiInitializer.HeaderSpace;
            DrawMethodsButton(rect, targetObject);
        }

        private void DrawEditButton(Rect rect, Object targetObject)
        {
            GUIContent content = new GUIContent(string.Empty, EditorGUIUtility.IconContent("editicon.sml").image,
                "Edit Script");
            
            EditorHeaderGuiInitializer.DrawHeaderButton(rect, content, () => 
                {
                    MonoScript script = MonoScript.FromMonoBehaviour((MonoBehaviour)targetObject);
                    if (script != null)
                    {
                        AssetDatabase.OpenAsset(script);
                    }
                    else
                    {
                        Debug.LogError($"Cannot open {script.name}");
                    }
                }
            );
        }

        private void DrawMethodsButton(Rect rect, Object targetObject)
        {
            MethodButtonAttributeHandler.CustomMethodInfo[] methods = 
                MethodButtonAttributeHandler.GetObjectMethods(target, ref _methodsToShow);
            if (methods == null)
            {
                return;
            }

            GUIContent content = new GUIContent(string.Empty,
                EditorGUIUtility.IconContent("d_Profiler.UIDetails").image,
                "Display Serialized Methods");
            
            EditorHeaderGuiInitializer.DrawHeaderButton(rect, content, () =>
                {
                    GenericMenu menu = new GenericMenu();
                
                    int len = methods.Length;
                    if (len == 0)
                    {
                        menu.AddItem(new GUIContent("No serialized methods to display"), false, () => {});
                    }
                    else
                    {
                        for (int i = 0, c = len; i < c; i++)
                        {
                            int index = i;
                            MethodButtonAttributeHandler.CustomMethodInfo method = methods[i];

                            menu.AddItem(new GUIContent($"{index}: {method.Signature}"), false,
                                () => method.Method.Invoke(target, null));
                        }
                    }

                    menu.ShowAsContext();
                }
            );
        }

        public override void OnInspectorGUI()
        {
            DrawInspectorFields();
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
                    // using (new EditorGUI.DisabledScope(true))
                    // {
                    //     EditorGUILayout.PropertyField(iterator, new GUIContent("Script",
                    //         showTooltip ? $"{tooltipAttribute.tooltip}" : null), true);
                    // }
                    
                    continue;
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