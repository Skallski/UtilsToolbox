using System;
using UnityEditor;
using UnityEngine;
using UtilsToolbox.Utils.UI;

namespace UtilsToolbox.Editor.CustomEditors.UI
{
    [CustomEditor(typeof(Button))]
    [CanEditMultipleObjects]
    public class ButtonEditor : UnityEditor.Editor
    {
        private SerializedProperty _state;
        private SerializedProperty _isInteractible;
        private SerializedProperty _events;
        
        private GUIContent _iconToolbarMinus;
        private GUIContent[] _eventTypes;
        private GUIContent _addButtonContent;
        
        private void OnEnable()
        {
            _state = serializedObject.FindProperty("_state");
            _isInteractible = serializedObject.FindProperty("_isInteractable");
            _events = serializedObject.FindProperty("_events");

            _addButtonContent = EditorGUIUtility.TrTextContent("Add New Button Event");
            
            _iconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"))
            {
                tooltip = "Remove all events in this list."
            };
            
            string[] eventNames = Enum.GetNames(typeof(Button.ButtonEventType));
            _eventTypes = new GUIContent[eventNames.Length];
            for (int i = 0, c = eventNames.Length; i < c; i++)
            {
                _eventTypes[i] = new GUIContent(eventNames[i]);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_isInteractible);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(_state);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0f, 0f, 0f, 0.3f));
            EditorGUILayout.Space();

            int toBeRemovedEntry = -1;
            Vector2 removeButtonSize = GUIStyle.none.CalcSize(_iconToolbarMinus);

            for (int i = 0, c = _events.arraySize; i < c; i++)
            {
                SerializedProperty delegateProp = _events.GetArrayElementAtIndex(i);
                SerializedProperty callbackProp = delegateProp.FindPropertyRelative("callback");
                SerializedProperty buttonEventProp = delegateProp.FindPropertyRelative("buttonEventType");
                GUIContent eventName = new GUIContent(buttonEventProp.enumDisplayNames[buttonEventProp.enumValueIndex]);
                
                EditorGUILayout.PropertyField(callbackProp, eventName);
                
                // remove button
                Rect callbackRect = GUILayoutUtility.GetLastRect();
                Rect removeButtonPos = new Rect(callbackRect.xMax - removeButtonSize.x - 8, callbackRect.y + 1, removeButtonSize.x, removeButtonSize.y);
                if (GUI.Button(removeButtonPos, _iconToolbarMinus, GUIStyle.none))
                {
                    toBeRemovedEntry = i;
                }

                EditorGUILayout.Space();
            }
            
            if (toBeRemovedEntry > -1)
            {
                _events.DeleteArrayElementAtIndex(toBeRemovedEntry);
            }
            
            // add new button event
            Rect btPosition = GUILayoutUtility.GetRect(_addButtonContent, GUI.skin.button);
            const float addButtonWidth = 200f;
            btPosition.x += (btPosition.width - addButtonWidth) / 2;
            btPosition.width = addButtonWidth;

            GUIContent buttonLabel = new GUIContent("Add Button Event",
                EditorGUIUtility.IconContent("d_EventTrigger Icon").image);
            
            if (_events.arraySize < _eventTypes.Length)
            {
                if (GUI.Button(btPosition, buttonLabel))
                {
                    ShowAddPointerEventMenu();
                }
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                GUI.Button(btPosition, buttonLabel);
                EditorGUI.EndDisabledGroup();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowAddPointerEventMenu()
        {
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < _eventTypes.Length; i++)
            {
                bool active = true;
                    
                for (int j = 0; j < _events.arraySize; j++)
                {
                    SerializedProperty delegateProp = _events.GetArrayElementAtIndex(j);
                    SerializedProperty buttonEventProp = delegateProp.FindPropertyRelative("buttonEventType");
                    if (buttonEventProp.enumValueIndex == i)
                    {
                        active = false;
                    }
                }
                
                if (active)
                {
                    int index = i;
                    
                    menu.AddItem(_eventTypes[i], false, () =>
                    {
                        _events.arraySize += 1;
                        SerializedProperty newDelegateProp = _events.GetArrayElementAtIndex(_events.arraySize - 1);
                        SerializedProperty buttonEventProp = newDelegateProp.FindPropertyRelative("buttonEventType");
                        buttonEventProp.enumValueIndex = index;
                        serializedObject.ApplyModifiedProperties();
                    });
                }
                else
                {
                    menu.AddDisabledItem(_eventTypes[i]);
                }
            }
                
            menu.ShowAsContext();
        }
    }
}