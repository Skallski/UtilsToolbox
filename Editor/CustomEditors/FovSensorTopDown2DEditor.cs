using System;
using SkalluUtils.Utils;
using SkalluUtils.Utils.FovSensorTopDown2D;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace SkalluUtils.CustomEditors
{
    [CustomEditor(typeof(FovSensorTopDown2D), true)]
    public class FovSensorTopDown2DEditor : Editor
    {
        private FovSensorTopDown2D _sensor;
        
        private static SerializedProperty ObstacleLayer;
        private static SerializedProperty TargetObject;
        private static SerializedProperty ScanRate;
        private static SerializedProperty Radius;
        private static SerializedProperty Angle;
        private static SerializedProperty Color;
        private static SerializedProperty Thickness;
        private static SerializedProperty TargetSpotted;
        private static SerializedProperty TargetInRange;
        private static SerializedProperty UseAdditionalZones;
        private static SerializedProperty AdditionalZones;

        private static bool UnwrapFovConeParams = true;
        private static bool UnwrapDebugParams = false;
        
        private static int ToolbarInt = 0;
        private static readonly string[] ToolbarHeaders = {"FoV Cone", "FoV Additional Zones"};

        private void OnEnable()
        {
            _sensor = target as FovSensorTopDown2D;
            
            EditorApplication.update += Update;
            
            ObstacleLayer = serializedObject.FindProperty("_obstacleLayer");
            TargetObject = serializedObject.FindProperty("_targetObject");
            ScanRate = serializedObject.FindProperty("_scanRate");
            Radius = serializedObject.FindProperty("_radius");
            Angle = serializedObject.FindProperty("_angle");
            Color = serializedObject.FindProperty("_color");
            Thickness = serializedObject.FindProperty("_thickness");
            TargetSpotted = serializedObject.FindProperty("_targetSpotted");
            TargetInRange = serializedObject.FindProperty("_targetInRange");
            UseAdditionalZones = serializedObject.FindProperty("_useAdditionalZones");
            AdditionalZones = serializedObject.FindProperty("_additionalZones");
        }

        private void OnDisable()
        {
            EditorApplication.update -= Update;
        }

        private void Update()
        {
            if (Application.isPlaying == false)
            {
                return;
            }
            
            //Debug.LogWarning("iks de");

            if (GUI.changed)
            {
                
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
        }

        public override void OnInspectorGUI()
        {
            if (_sensor == null)
            {
                return;
            }
            
            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            EditorGUILayout.PropertyField(ObstacleLayer);
            EditorGUILayout.PropertyField(TargetObject);
            EditorGUILayout.PropertyField(ScanRate);
            
            EditorGUIExtensions.HorizontalLine(new Vector2(10f, 10f));
            
            ToolbarInt = GUILayout.Toolbar(ToolbarInt, ToolbarHeaders);
            switch (ToolbarInt)
            {
                case 0:
                {
                    UnwrapFovConeParams = EditorGUILayout.BeginFoldoutHeaderGroup(UnwrapFovConeParams, "Field of View Cone");
                    if (UnwrapFovConeParams)
                    {
                        EditorGUILayout.PropertyField(Radius);
                        EditorGUILayout.PropertyField(Angle);
                        EditorGUILayout.PropertyField(Color);
                        EditorGUILayout.PropertyField(Thickness);
                    }
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    
                    break;
                }
            
                case 1:
                {
                    EditorGUILayout.PropertyField(UseAdditionalZones);
                    if (EditorGUILayout.BeginFadeGroup(UseAdditionalZones.boolValue ? 1 : 0))
                    {
                        EditorGUILayout.PropertyField(AdditionalZones);
                    }
                    EditorGUILayout.EndFadeGroup();
                    
                    break;
                }
            }
            
            EditorGUIExtensions.HorizontalLine(new Vector2(10f, 10f));
            
            UnwrapDebugParams = EditorGUILayout.BeginFoldoutHeaderGroup(UnwrapDebugParams, "Debug");
            if (UnwrapDebugParams)
            {
                EditorGUILayout.PropertyField(TargetSpotted);
            
                if (UseAdditionalZones.boolValue)
                {
                    var zones = _sensor.GetZones();
                    var len = zones.Count;
                    if (len > 0)
                    {
                        GUI.enabled = false;
                        for (var i = 0; i < len; i++)
                        {
                            EditorGUILayout.Toggle($"Zone {i + 1}: Target spotted", zones[i].TargetSpotted);
                        }
                        GUI.enabled = true;
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Additional zones debug info will be displayed here when zones will be created", MessageType.Info);
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            
            if (GUI.changed)
            {
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
        }

        private void OnSceneGUI()
        {
            if (_sensor == null)
            {
                return;
            }
            
            var agentPosition = _sensor.transform.position;
            DrawViewCone(agentPosition);
            DrawAdditionalZones(agentPosition);
            
            // draws line from character to spotted target
            if (Application.isPlaying)
            {
                if (TargetInRange.boolValue)
                {
                    Handles.color = _sensor.TargetSpotted ? UnityEngine.Color.green : UnityEngine.Color.red;
                    Handles.DrawLine(agentPosition, _sensor.TargetTransform.position, Thickness.floatValue);
                }
                
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
        }
        
        private void DrawViewCone(Vector3 agentPosition)
        {
            // draws view radius
            DrawZone(agentPosition, Radius.floatValue, Thickness.floatValue, Color.colorValue);

            // calculates and draws view angle cone
            var eulerAngles = _sensor.transform.eulerAngles;
            var viewAngleLeft = CalculateDirectionFromAngle((-Angle.intValue + 180) * 0.5f, eulerAngles.z); // left side angle: \|
            var viewAngleRight = CalculateDirectionFromAngle((Angle.intValue + 180) * 0.5f, eulerAngles.z); // right side angle: |/
            
            Handles.DrawLine(agentPosition, agentPosition + viewAngleLeft * Radius.floatValue, Thickness.floatValue);
            Handles.DrawLine(agentPosition, agentPosition + viewAngleRight * Radius.floatValue, Thickness.floatValue);
        }

        private void DrawAdditionalZones(Vector3 agentPosition)
        {
            if (!UseAdditionalZones.boolValue)
            {
                return;
            }

            foreach (var zone in _sensor.GetZones())
            {
                DrawZone(agentPosition, zone.Radius, zone.Thickness, zone.Color);
            }
        }

        private static void DrawZone(Vector3 agentPosition, float radius, float thickness, Color color)
        {
            Handles.color = color;
            Handles.DrawWireArc(agentPosition, Vector3.forward, Vector3.up, 360f, radius, thickness);
        }

        /// <summary>
        /// Calculated direction from angle
        /// </summary>
        /// <param name="inputAngle"> provided angle </param>
        /// <param name="inputEulerAngleZ"> provided euler Z angle </param>
        private static Vector3 CalculateDirectionFromAngle(float inputAngle, float inputEulerAngleZ)
        {
            var newAngle = inputAngle - inputEulerAngleZ;
            return new Vector3(Mathf.Sin(newAngle * Mathf.Deg2Rad), Mathf.Cos(newAngle * Mathf.Deg2Rad), 0f);
        }
    }
}