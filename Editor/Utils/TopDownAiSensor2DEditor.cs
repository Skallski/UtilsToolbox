using SkalluUtils.Utils;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace SkalluUtils.Editor.Utils
{
    [CustomEditor(typeof(TopDownAiSensor2D)), CanEditMultipleObjects]
    public class TopDownAiSensor2DEditor : UnityEditor.Editor
    {
        private TopDownAiSensor2D _aiSensor;
        
        #region PROPERTIES AND GUI CONTENT
        private SerializedProperty
            _viewOuterRadius, _viewInnerRadius, _viewAngle, _useSpecialZones, _safeZoneRadius, _attackRangeRadius, _sensorTick, _obstacleLayerMask, _targetObject,
            _mainColor, _safeZoneColor, _attackRangeColor, _thickness,
            _targetInsideViewOuterRadius, _targetSpotted, _targetInsideSafeZone, _targetInsideAttackRange;
        
        private static readonly GUIContent ViewOuterRadiusContent = new GUIContent("Outer view radius", "Circular detection area max border");
        private static readonly GUIContent ViewInnerRadiusContent = new GUIContent("Inner view radius", "Circular detection area min border");
        private static readonly GUIContent ViewAngleContent = new GUIContent("View angle", "detection angle (in degrees)");
        private static readonly GUIContent SafeZoneRadiusContent = new GUIContent("Safe zone radius", "(optional) Safe zone area radius");
        private static readonly GUIContent AttackRangeRadiusContent = new GUIContent("Attack range radius", "(optional) Attack range area");
        private static readonly GUIContent SensorTickContent = new GUIContent("Update interval", "aiSensor update tick");
        #endregion

        #region GROUP WRAPPING RELATED FIELDS
        private bool _showSensorParameters = true; // is "Sensor Parameters" foldout header group unwrapped
        private bool _showSceneGuiParameters = true; // is "Scene GUI Parameters" foldout header group unwrapped
        private bool _showSensorFlags = false; // is "Sensor Flags" foldout header group unwrapped
        #endregion

        private void OnEnable()
        {
            _aiSensor = (TopDownAiSensor2D) target;

            _viewOuterRadius = serializedObject.FindProperty("_viewOuterRadius");
            _viewInnerRadius = serializedObject.FindProperty("_viewInnerRadius");
            _viewAngle = serializedObject.FindProperty("_viewAngle");
            _useSpecialZones = serializedObject.FindProperty("_useSpecialZones");
            _safeZoneRadius = serializedObject.FindProperty("_safeZoneRadius");
            _attackRangeRadius = serializedObject.FindProperty("_attackRangeRadius");
            _sensorTick = serializedObject.FindProperty("_sensorTick");
            _obstacleLayerMask = serializedObject.FindProperty("_obstacleLayerMask");
            _targetObject = serializedObject.FindProperty("_targetObject");
            
            _mainColor = serializedObject.FindProperty("_mainColor");
            _safeZoneColor = serializedObject.FindProperty("_safeZoneColor");
            _attackRangeColor = serializedObject.FindProperty("_attackRangeColor");
            _thickness = serializedObject.FindProperty("_thickness");

            _targetInsideViewOuterRadius = serializedObject.FindProperty("_targetInsideViewOuterRadius");
            _targetSpotted = serializedObject.FindProperty("_targetSpotted");
            _targetInsideSafeZone = serializedObject.FindProperty("_targetInsideSafeZone");
            _targetInsideAttackRange = serializedObject.FindProperty("_targetInsideAttackRange");
        }

        public override void OnInspectorGUI()
        {
            if (_aiSensor == null) return;

            // default "script" object field
            EditorGUIUtils.DefaultScriptField(MonoScript.FromMonoBehaviour((MonoBehaviour)target));
            
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            
            #region SENSOR PARAMETERS GROUP
            _showSensorParameters = EditorGUILayout.BeginFoldoutHeaderGroup(_showSensorParameters, "Main sensor parameters");
            if (_showSensorParameters)
            {
                EditorGUIUtils.PropertySliderField(_viewOuterRadius, 0, 20, ViewOuterRadiusContent);
                EditorGUIUtils.PropertySliderField(_viewInnerRadius, 0, 10, ViewInnerRadiusContent);
                EditorGUIUtils.PropertySliderField(_viewAngle, 0, 360, ViewAngleContent);

                // shows "Special Zones" main parameters
                EditorGUILayout.PropertyField(_useSpecialZones);
                if (EditorGUILayout.BeginFadeGroup(_useSpecialZones.boolValue ? 1 : 0))
                {
                    EditorGUIUtils.PropertySliderField(_safeZoneRadius, 0, 10, SafeZoneRadiusContent);
                    EditorGUIUtils.PropertySliderField(_attackRangeRadius, 0, 10, AttackRangeRadiusContent);
                }
                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.Space();

                EditorGUIUtils.PropertySliderField(_sensorTick, 0.001f, 1, SensorTickContent);
                EditorGUILayout.PropertyField(_obstacleLayerMask);
                EditorGUILayout.PropertyField(_targetObject);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region SCENE GUI PARAMETERS GROUP
            _showSceneGuiParameters = EditorGUILayout.BeginFoldoutHeaderGroup(_showSceneGuiParameters, "Scene GUI Parameters");
            if (_showSceneGuiParameters)
            {
                 EditorGUIUtils.PropertySliderField(_thickness, 0.5f, 2, default);
                 EditorGUILayout.PropertyField(_mainColor);

                 // shows "Special Zones" visual parameters
                if (EditorGUILayout.BeginFadeGroup(_useSpecialZones.boolValue ? 1 : 0))
                {
                    EditorGUILayout.PropertyField(_safeZoneColor);
                    EditorGUILayout.PropertyField(_attackRangeColor);
                }
                EditorGUILayout.EndFadeGroup();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region SENSOR FLAGS GROUP
            _showSensorFlags = EditorGUILayout.BeginFoldoutHeaderGroup(_showSensorFlags, "Sensor Flags");
            if (_showSensorFlags)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(_targetInsideViewOuterRadius);
                EditorGUILayout.PropertyField(_targetSpotted);

                // shows "Special Zones" debug checks
                if (EditorGUILayout.BeginFadeGroup(_useSpecialZones.boolValue ? 1 : 0))
                {
                    EditorGUILayout.PropertyField(_targetInsideSafeZone);
                    EditorGUILayout.PropertyField(_targetInsideAttackRange);
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
            if (_aiSensor == null) return;
            
            var pos = _aiSensor.transform.position;

            // draws outer and inner view radius
            DrawZone(pos, 360f, _viewOuterRadius.floatValue, _thickness.floatValue, _mainColor.colorValue);
            DrawZone(pos, 360f, _viewInnerRadius.floatValue, _thickness.floatValue, _mainColor.colorValue);

            // calculates and draws view angle cone
            var eulerAngles = _aiSensor.transform.eulerAngles;
            var viewAngleLeft = CalculateDirectionFromAngle((-_viewAngle.floatValue + 180) * 0.5f, eulerAngles.z); // left view angle: \|
            var viewAngleRight = CalculateDirectionFromAngle((_viewAngle.floatValue + 180) * 0.5f, eulerAngles.z); // right view angle: |/
            
            Handles.color = _mainColor.colorValue;
            Handles.DrawLine(pos, pos + viewAngleLeft * _viewOuterRadius.floatValue, _thickness.floatValue);
            Handles.DrawLine(pos, pos + viewAngleRight * _viewOuterRadius.floatValue, _thickness.floatValue);
            
            // draws special zones if used
            if (_useSpecialZones.boolValue is true)
            {
                DrawZone(pos, 360f, _safeZoneRadius.floatValue, _thickness.floatValue, _safeZoneColor.colorValue); // draws safe zone radius
                DrawZone(pos, 360f, _attackRangeRadius.floatValue, _thickness.floatValue, _attackRangeColor.colorValue); // draws attack range radius
            }
            
            // draws line from character to spotted target
            if (_aiSensor.TargetInsideViewOuterRadius)
            {
                Handles.color = _aiSensor.TargetSpotted ? Color.green : Color.red;
                Handles.DrawLine(pos, _aiSensor.TargetObject.transform.position, _thickness.floatValue);
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