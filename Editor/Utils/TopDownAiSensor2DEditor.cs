using SkalluUtils.Utils;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;

namespace SkalluUtils.Editor.Utils
{
    [CustomEditor(typeof(TopDownAiSensor2D)), CanEditMultipleObjects]
    public class TopDownAiSensor2DEditor : UnityEditor.Editor
    {
        private TopDownAiSensor2D aiSensor;
        
        #region PROPERTIES AND GUI CONTENT
        private SerializedProperty
            viewOuterRadius, viewInnerRadius, viewAngle, useSpecialZones, safeZoneRadius, attackRangeRadius, sensorTick, obstacleLayerMask, targetObject,
            mainColor, safeZoneColor, attackRangeColor, thickness,
            targetInsideViewOuterRadius, targetSpotted, targetInsideSafeZone, targetInsideAttackRange;
        
        private static readonly GUIContent ViewOuterRadiusContent = new GUIContent("Outer view radius", "Circular detection area max border");
        private static readonly GUIContent ViewInnerRadiusContent = new GUIContent("Inner view radius", "Circular detection area min border");
        private static readonly GUIContent ViewAngleContent = new GUIContent("View angle", "detection angle (in degrees)");
        private static readonly GUIContent SafeZoneRadiusContent = new GUIContent("Safe zone radius", "(optional) Safe zone area radius");
        private static readonly GUIContent AttackRangeRadiusContent = new GUIContent("Attack range radius", "(optional) Attack range area");
        private static readonly GUIContent SensorTickContent = new GUIContent("Update interval", "aiSensor update tick");
        #endregion

        #region GROUP WRAPPING RELATED FIELDS
        private bool showSensorParameters = true; // is "Sensor Parameters" foldout header group unwrapped
        private bool showSceneGuiParameters = true; // is "Scene GUI Parameters" foldout header group unwrapped
        private bool showSensorFlags = false; // is "Sensor Flags" foldout header group unwrapped
        #endregion

        private void OnEnable()
        {
            aiSensor = (TopDownAiSensor2D) target;

            viewOuterRadius = serializedObject.FindProperty("viewOuterRadius");
            viewInnerRadius = serializedObject.FindProperty("viewInnerRadius");
            viewAngle = serializedObject.FindProperty("viewAngle");
            useSpecialZones = serializedObject.FindProperty("useSpecialZones");
            safeZoneRadius = serializedObject.FindProperty("safeZoneRadius");
            attackRangeRadius = serializedObject.FindProperty("attackRangeRadius");
            sensorTick = serializedObject.FindProperty("sensorTick");
            obstacleLayerMask = serializedObject.FindProperty("obstacleLayerMask");
            targetObject = serializedObject.FindProperty("targetObject");
            
            mainColor = serializedObject.FindProperty("mainColor");
            safeZoneColor = serializedObject.FindProperty("safeZoneColor");
            attackRangeColor = serializedObject.FindProperty("attackRangeColor");
            thickness = serializedObject.FindProperty("thickness");

            targetInsideViewOuterRadius = serializedObject.FindProperty("targetInsideViewOuterRadius");
            targetSpotted = serializedObject.FindProperty("targetSpotted");
            targetInsideSafeZone = serializedObject.FindProperty("targetInsideSafeZone");
            targetInsideAttackRange = serializedObject.FindProperty("targetInsideAttackRange");
        }

        public override void OnInspectorGUI()
        {
            if (aiSensor == null) return;

            // default "script" object field
            EditorGUIUtils.DefaultScriptField(MonoScript.FromMonoBehaviour((MonoBehaviour)target));
            
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            
            #region SENSOR PARAMETERS GROUP
            showSensorParameters = EditorGUILayout.BeginFoldoutHeaderGroup(showSensorParameters, "Main sensor parameters");
            if (showSensorParameters)
            {
                EditorGUIUtils.PropertySliderField(viewOuterRadius, 0, 20, ViewOuterRadiusContent);
                EditorGUIUtils.PropertySliderField(viewInnerRadius, 0, 10, ViewInnerRadiusContent);
                EditorGUIUtils.PropertySliderField(viewAngle, 0, 360, ViewAngleContent);

                // shows "Special Zones" main parameters
                EditorGUILayout.PropertyField(useSpecialZones);
                if (EditorGUILayout.BeginFadeGroup(useSpecialZones.boolValue ? 1 : 0))
                {
                    EditorGUIUtils.PropertySliderField(safeZoneRadius, 0, 10, SafeZoneRadiusContent);
                    EditorGUIUtils.PropertySliderField(attackRangeRadius, 0, 10, AttackRangeRadiusContent);
                }
                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.Space();

                EditorGUIUtils.PropertySliderField(sensorTick, 0.001f, 1, SensorTickContent);
                EditorGUILayout.PropertyField(obstacleLayerMask);
                EditorGUILayout.PropertyField(targetObject);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region SCENE GUI PARAMETERS GROUP
            showSceneGuiParameters = EditorGUILayout.BeginFoldoutHeaderGroup(showSceneGuiParameters, "Scene GUI Parameters");
            if (showSceneGuiParameters)
            {
                 EditorGUIUtils.PropertySliderField(thickness, 0.5f, 2, default);
                 EditorGUILayout.PropertyField(mainColor);

                 // shows "Special Zones" visual parameters
                if (EditorGUILayout.BeginFadeGroup(useSpecialZones.boolValue ? 1 : 0))
                {
                    EditorGUILayout.PropertyField(safeZoneColor);
                    EditorGUILayout.PropertyField(attackRangeColor);
                }
                EditorGUILayout.EndFadeGroup();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region SENSOR FLAGS GROUP
            showSensorFlags = EditorGUILayout.BeginFoldoutHeaderGroup(showSensorFlags, "Sensor Flags");
            if (showSensorFlags)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(targetInsideViewOuterRadius);
                EditorGUILayout.PropertyField(targetSpotted);

                // shows "Special Zones" debug checks
                if (EditorGUILayout.BeginFadeGroup(useSpecialZones.boolValue ? 1 : 0))
                {
                    EditorGUILayout.PropertyField(targetInsideSafeZone);
                    EditorGUILayout.PropertyField(targetInsideAttackRange);
                }
                EditorGUILayout.EndFadeGroup();
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            #endregion

            // apply modified properties and repaint
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
                InternalEditorUtility.RepaintAllViews();
        }

        private void OnSceneGUI()
        {
            if (aiSensor == null) return;
            
            var pos = aiSensor.transform.position;

            // draws outer and inner view radius
            DrawZone(pos, 360f, viewOuterRadius.floatValue, thickness.floatValue, mainColor.colorValue);
            DrawZone(pos, 360f, viewInnerRadius.floatValue, thickness.floatValue, mainColor.colorValue);

            // calculates and draws view angle cone
            var eulerAngles = aiSensor.transform.eulerAngles;
            var viewAngleLeft = CalculateDirectionFromAngle((-viewAngle.floatValue + 180) * 0.5f, eulerAngles.z); // left view angle: \|
            var viewAngleRight = CalculateDirectionFromAngle((viewAngle.floatValue + 180) * 0.5f, eulerAngles.z); // right view angle: |/
            
            Handles.color = mainColor.colorValue;
            Handles.DrawLine(pos, pos + viewAngleLeft * viewOuterRadius.floatValue, thickness.floatValue);
            Handles.DrawLine(pos, pos + viewAngleRight * viewOuterRadius.floatValue, thickness.floatValue);
            
            // draws special zones if used
            if (useSpecialZones.boolValue is true)
            {
                DrawZone(pos, 360f, safeZoneRadius.floatValue, thickness.floatValue, safeZoneColor.colorValue); // draws safe zone radius
                DrawZone(pos, 360f, attackRangeRadius.floatValue, thickness.floatValue, attackRangeColor.colorValue); // draws attack range radius
            }
            
            // draws line from character to spotted target
            if (aiSensor.TargetInsideViewOuterRadius)
            {
                Handles.color = aiSensor.TargetSpotted ? Color.green : Color.red;
                Handles.DrawLine(pos, aiSensor.TargetObject.transform.position, thickness.floatValue);
            }
        }

        /// <summary>
        /// Draws Field of View zone wire arc in certain color
        /// </summary>
        /// <param name="center"> center position </param>
        /// <param name="angle"> angle </param>
        /// <param name="radius"> radius </param>
        /// <param name="lineThickness"> handles thickness </param>
        /// <param name="color"> handles color </param>
        private static void DrawZone(Vector3 center, float angle, float radius, float lineThickness, Color color)
        {
            Handles.color = color;
            Handles.DrawWireArc(center, Vector3.forward, Vector3.up, angle, radius, lineThickness);
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