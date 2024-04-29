using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SkalluUtils.Editor.Utils
{
    public static class EditorHeaderGuiInitializer
    {
        public static readonly float HeaderSpace = 20;
        
        public static Action<Rect, Object> OnDrawHeaderGUIEvent;

        public static void InitializeHeaderGUI(MonoBehaviour targetMonoBehaviour)
        {
            FieldInfo fieldInfo = typeof(EditorGUIUtility).GetField("s_EditorHeaderItemsMethods", 
                BindingFlags.NonPublic | BindingFlags.Static);
            if (fieldInfo == null || fieldInfo.GetValue(targetMonoBehaviour) is not IList value)
            {
                return;
            }

            Type headerItemDelegateType = value.GetType().GetGenericArguments().FirstOrDefault();
            if (headerItemDelegateType == null)
            {
                return;
            }
        
            MethodInfo drawHeaderItemMethodInfo = new Func<Rect, Object[], bool>(DrawHeaderItem).Method;
            Delegate headerItemDelegate = Delegate.CreateDelegate(headerItemDelegateType, drawHeaderItemMethodInfo);
            if (value.Contains(headerItemDelegate) == false)
            {
                value.Add(headerItemDelegate);
            }
        }
        
        private static bool DrawHeaderItem(Rect rect, Object[] targets)
        {
            Object targetObject = targets.FirstOrDefault();
            if (targetObject == null)
            {
                return false;
            }

            Type targetType = targetObject.GetType();
            if (typeof(MonoBehaviour).IsAssignableFrom(targetType) == false)
            {
                return false;
            }
            
            OnDrawHeaderGUIEvent?.Invoke(rect, targetObject);
            return true;
        }

        private static GUIStyle IconButtonGUIStyle()
        {
            return new GUIStyle(GUI.skin.GetStyle("IconButton"));
        }

        public static void DrawHeaderButton(Rect rect, GUIContent content, Action onClick)
        {
            if (GUI.Button(rect, content, IconButtonGUIStyle()))
            {
                onClick?.Invoke();
            }
        }

        public static void DrawHeaderButton(Rect rect, string label, Texture2D icon, string tooltip, Action onClick)
        {
            DrawHeaderButton(rect, new GUIContent(label, icon, tooltip), onClick);
        }
    }
}