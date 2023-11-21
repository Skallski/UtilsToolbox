using System.Reflection;
using SkalluUtils.Tools;
using SkalluUtils.Utils.FieldOfView;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.CustomEditors.FieldOfView
{
    [CustomEditor(typeof(Fov2D), true)]
    public class Fov2DEditor : Editor
    {
        private Fov2D _fov;
        
        private SerializedProperty _targetObject;
        private SerializedProperty _scanRate;
        private SerializedProperty _seeThroughObstacles;
        private SerializedProperty _obstacleLayer;
        private SerializedProperty _radius;
        private SerializedProperty _angle;
        private SerializedProperty _fovHandlesType;
        private SerializedProperty _useCustomColor;
        private SerializedProperty _customColor;
        
        private static int ToolbarInt = 0;
        private static readonly string[] ToolbarHeaders = {"FOV cone", "Obstacles"};
        
        private void OnEnable()
        {
            _fov = target as Fov2D;
            
            EditorApplication.update += OnEditorUpdate;
            
            _targetObject = serializedObject.FindProperty("_targetObject");
            _scanRate = serializedObject.FindProperty("_scanRate");
            _seeThroughObstacles = serializedObject.FindProperty("_seeThroughObstacles");
            _obstacleLayer = serializedObject.FindProperty("_obstacleLayer");
            _radius = serializedObject.FindProperty("_radius");
            _angle = serializedObject.FindProperty("_angle");
            _fovHandlesType = serializedObject.FindProperty("_fovHandlesType");
            _useCustomColor = serializedObject.FindProperty("_useCustomColor");
            _customColor = serializedObject.FindProperty("_customColor");
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        public override void OnInspectorGUI()
        {
            if (_fov == null)
            {
                return;
            }
            
            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            #region DRAW INSPECTOR
            EditorGUILayout.PropertyField(_targetObject);
            EditorGUILayout.PropertyField(_scanRate);
            
            if (Application.isPlaying)
            {
                if (_targetObject.objectReferenceValue != null)
                {
                    if (GUILayout.Button("Test Setup"))
                    {
                        _fov.GetType()
                            .GetMethod("Setup", BindingFlags.Instance | BindingFlags.NonPublic)
                            ?.Invoke(_fov, new object[] { _targetObject.objectReferenceValue as GameObject });
                    }
                }
            }

            HorizontalLine();
            
            ToolbarInt = GUILayout.Toolbar(ToolbarInt, ToolbarHeaders);
            EditorGUILayout.Space();
            
            switch (ToolbarInt)
            {
                case 0:
                {
                    EditorGUILayout.PropertyField(_fovHandlesType);
                    EditorGUILayout.PropertyField(_useCustomColor);

                    if (_useCustomColor.boolValue)
                    {
                        EditorGUILayout.PropertyField(_customColor);
                    }
                    
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(_radius);
                    EditorGUILayout.PropertyField(_angle);
                    
                    break;
                }
            
                case 1:
                {
                    EditorGUILayout.PropertyField(_seeThroughObstacles);
                    if (_seeThroughObstacles.boolValue == false)
                    {
                        EditorGUILayout.PropertyField(_obstacleLayer);
                    }
                    
                    break;
                }
            }
            
            if (Application.isPlaying)
            {
                HorizontalLine();
                
                GUI.enabled = false;
                EditorGUILayout.Toggle("Target in range", _fov.TargetInRange);
                EditorGUILayout.Toggle("Target spotted", _fov.TargetSpotted);
                GUI.enabled = true;
            }

            void HorizontalLine()
            {
                EditorGUILayout.Space(10f);
                EditorGUI.DrawRect(
                    EditorGUILayout.GetControlRect(false, 1), new Color(0f, 0f, 0f, 0.3f));
                EditorGUILayout.Space(10f);
            }
            #endregion
            
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            
            if (GUI.changed)
            {
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
        }

        private void OnSceneGUI()
        {
            if (_fov == null)
            {
                return;
            }
            
            var transform = _fov.transform;

            if (_useCustomColor.boolValue)
            {
                Handles.color = _customColor.colorValue;
            }
            else
            {
                if (Application.isPlaying)
                {
                    Handles.color = _fov.TargetSpotted 
                        ? new Color(1, 0, 0, 0.3f) 
                        : new Color(0, 1, 0, 0.3f);
                }
                else
                {
                    Handles.color = new Color(0, 1, 0, 0.3f);
                }
            }
            
            switch ((Fov2D.HandlesType) _fovHandlesType.enumValueIndex)
            {
                case Fov2D.HandlesType.Solid:
                {
                    Handles.DrawSolidArc(
                        transform.position,
                        Vector3.forward, 
                        Quaternion.AngleAxis(-_angle.intValue / 2f, Vector3.forward) * Vector3.right,
                        _angle.intValue,
                        _radius.floatValue);
                    
                    break;
                }

                case Fov2D.HandlesType.Wire:
                {
                    var position = transform.position;
                    const float handlesThickness = 2f;
                    
                    Handles.DrawWireArc(
                        position,
                        Vector3.forward, 
                        Quaternion.AngleAxis(-_angle.intValue / 2f, Vector3.forward) * Vector3.right,
                        _angle.intValue,
                        _radius.floatValue, 
                        handlesThickness);
                    
                    // draw left (\|) and right (|/) sides of view angle
                    Vector3 GetDirectionFromAngle(float angle, float eulerAngleZ)
                    {
                        var newAngle = angle - eulerAngleZ;
                        return new Vector3(Mathf.Sin(newAngle * Mathf.Deg2Rad), Mathf.Cos(newAngle * Mathf.Deg2Rad), 0f);
                    }
                    
                    var eulerAngles = transform.eulerAngles;
                    var viewAngleLeft = GetDirectionFromAngle((-_angle.intValue + 180) * 0.5f, eulerAngles.z);
                    var viewAngleRight = GetDirectionFromAngle((_angle.intValue + 180) * 0.5f, eulerAngles.z);
                    
                    Handles.DrawLine(position, position + viewAngleLeft * _radius.floatValue, handlesThickness);
                    Handles.DrawLine(position, position + viewAngleRight * _radius.floatValue, handlesThickness);
                    
                    break;
                }
            }
        }

        private void OnEditorUpdate()
        {
            if (Application.isPlaying == false)
            {
                return;
            }
            
            if (GUI.changed)
            {
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
        }
    }
}