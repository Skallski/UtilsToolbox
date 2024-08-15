using UnityEditor;
using UnityEngine;
using UtilsToolbox.Utils.Optimization.Culling;

namespace UtilsToolbox.Editor.CustomEditors
{
    [CustomEditor(typeof(ObjectCuller))]
    public class ObjectCullerEditor : UnityEditor.Editor
    {
        private ObjectCuller _objectCuller;
        
        private SerializedProperty _cullingBoundingBox;
        private SerializedProperty _isObjectMovable;
        private SerializedProperty _isObjectVisible;
        private SerializedProperty _onBecameVisible;
        private SerializedProperty _onBecameInvisible;
        
        private void OnEnable()
        {
            _objectCuller = target as ObjectCuller;
            
            _cullingBoundingBox = serializedObject.FindProperty("_cullingBoundingBox");
            _isObjectMovable = serializedObject.FindProperty("_isObjectMovable");
            _isObjectVisible = serializedObject.FindProperty("_isObjectVisible");
            _onBecameVisible = serializedObject.FindProperty("_onBecameVisible");
            _onBecameInvisible = serializedObject.FindProperty("_onBecameInvisible");
        }

        public override void OnInspectorGUI()
        {
            if (_objectCuller == null)
            {
                return;
            }
            
            EditorGUILayout.PropertyField(_cullingBoundingBox);
            EditorGUILayout.PropertyField(_isObjectMovable);

            GUI.enabled = false;
            EditorGUILayout.PropertyField(_isObjectVisible);
            GUI.enabled = true;
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_onBecameVisible);
            EditorGUILayout.PropertyField(_onBecameInvisible);
        }
    }
}