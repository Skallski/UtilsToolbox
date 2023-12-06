using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace SkalluUtils.CustomEditors.UI
{
    [CustomEditor(typeof(Utils.UI.Button))]
    [CanEditMultipleObjects]
    public class ButtonEditor : Editor
    {
        private Utils.UI.Button _button;

        private SerializedProperty _state;
        private SerializedProperty _onPointerEnter;
        private SerializedProperty _onPointerDown;
        private SerializedProperty _onPointerUp;
        private SerializedProperty _onPointerExit;

        private bool _onPointerEnterUnfolded;
        private bool _onPointerDownUnfolded;
        private bool _onPointerUpUnfolded;
        private bool _onPointerExitUnfolded;

        private bool _methodsShown = false;

        private static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;

        private void OnEnable()
        {
            _button = target as Utils.UI.Button;

            _state = serializedObject.FindProperty("_state");
            _onPointerEnter = serializedObject.FindProperty("_onPointerEnter");
            _onPointerDown = serializedObject.FindProperty("_onPointerDown");
            _onPointerUp = serializedObject.FindProperty("_onPointerUp");
            _onPointerExit = serializedObject.FindProperty("_onPointerExit");
        }

        public override void OnInspectorGUI()
        {
            if (_button == null)
            {
                return;
            }
            
            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(_state);
            GUI.enabled = true;
            
            #region POINTER EVENTS VISUALISATION
            if (Application.isPlaying)
            {
                EditorGUILayout.Space();
                GUIStyle oldStyle = GUI.skin.button;
                GUIStyle style = new GUIStyle(GUI.skin.button)
                {
                    fixedHeight = 13f,
                    fontSize = 10
                };

                GUI.skin.button = style;
                if (GUILayout.Button(new GUIContent("Visualise Pointer Events", 
                        EditorGUIUtility.IconContent(_methodsShown ? "PreviewCollapse" : "PreviewExpand").image)))
                {
                    _methodsShown = !_methodsShown;
                }

                GUI.skin.button = oldStyle;
                if (_methodsShown)
                {
                    object[] param = new object[] { };
                    Type type = _button.GetType();

                    if (GUILayout.Button("OnPointerEnter"))
                    {
                        type.GetMethod("OnPointerEnterInternal", Flags)?.Invoke(_button, param);
                    }
                
                    if (GUILayout.Button("OnPointerDown"))
                    {
                        type.GetMethod("OnPointerDownInternal", Flags)?.Invoke(_button, param);
                    }
                
                    if (GUILayout.Button("OnPointerUp"))
                    {
                        type.GetMethod("OnPointerUpInternal", Flags)?.Invoke(_button, param);
                    }
                
                    if (GUILayout.Button("OnPointerExit"))
                    {
                        type.GetMethod("OnPointerExitInternal", Flags)?.Invoke(_button, param);
                    }
                }
            }
            #endregion

            EditorGUILayout.Space();
            EditorGUI.DrawRect(
                EditorGUILayout.GetControlRect(false, 1), new Color(0f, 0f, 0f, 0.3f));
            EditorGUILayout.Space();
            
            #region DISPLAY BUTTON EVENTS
            EditorGUILayout.LabelField("Button Events", 
                new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold },
                GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();

            void DisplayButtonEvent(ref bool unfolded, string label, SerializedProperty property)
            {
                FieldInfo propertyFieldInfo = target.GetType().GetField(property.propertyPath,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (propertyFieldInfo == null)
                {
                    return;
                }

                object fieldValue = propertyFieldInfo.GetValue(target);
                if (!(fieldValue is UnityEvent buttonEvent))
                {
                    return;
                }

                if (buttonEvent.GetPersistentEventCount() > 0)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(property);
                    EditorGUILayout.Space();
                }
                else
                {
                    if (unfolded)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(property);
                        EditorGUILayout.Space();
                    }
                    else
                    {
                        if (GUILayout.Button(label))
                        {
                            unfolded = !unfolded;
                        }
                    }
                }
            }
            
            DisplayButtonEvent(ref _onPointerEnterUnfolded, "OnPointerEnter()", _onPointerEnter);
            DisplayButtonEvent(ref _onPointerDownUnfolded, "OnPointerDown()", _onPointerDown);
            DisplayButtonEvent(ref _onPointerUpUnfolded, "OnPointerUp()", _onPointerUp);
            DisplayButtonEvent(ref _onPointerExitUnfolded, "OnPointerExit()", _onPointerExit);
            #endregion
            
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}