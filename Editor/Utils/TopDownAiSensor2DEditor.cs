using SkalluUtils.Utils;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace SkalluUtils.Editor.Utils
{
    [CustomEditor(typeof(TopDownAiSensor2D)), CanEditMultipleObjects]
    public class TopDownAiSensor2DEditor : UnityEditor.Editor
    {
        private TopDownAiSensor2D aiSensor;

        #region GROUP WRAPPING RELATED FIELDS
        private bool showFovParameters = true; // is "FOV Parameters" foldout header group unwrapped
        private bool showFovEditorParameters = true; // is "FOV Editor Parameters" foldout header group unwrapped
        private bool showFovChecks; // is "FOV Zone Checks" foldout header group unwrapped
        private SerializedProperty useSpecialZones;
        #endregion

        private void OnEnable()
        {
            aiSensor = (TopDownAiSensor2D) target;
            useSpecialZones = serializedObject.FindProperty("useSpecialZones");
        }

        public override void OnInspectorGUI()
        {
            if (aiSensor == null) return;
            
            // default "script" object field
            EditorGUIUtils.DefaultScriptField(MonoScript.FromMonoBehaviour((MonoBehaviour)target));
            
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            
            #region FOV PARAMETERS GROUP
            showFovParameters = EditorGUILayout.BeginFoldoutHeaderGroup(showFovParameters, "Main parameters");
            if (showFovParameters)
            {
                EditorGUIUtils.PropertySliderField(serializedObject.FindProperty("viewOuterRadius"), 0, 20,
                    new GUIContent("Outer view radius", "Area that determines the ability to detect target within it, provided that it is also within the viewing angle cone"));
                
                EditorGUIUtils.PropertySliderField(serializedObject.FindProperty("viewInnerRadius"), 0, 10,
                    new GUIContent("Inner view radius", "The minimum area that determines the ability to detect target within it"));
                
                EditorGUIUtils.PropertySliderField(serializedObject.FindProperty("viewAngle"), 0, 360,
                    new GUIContent("View angle", "Angle (in degrees), which determines the ability to spot objects within its area"));

                // shows "Special Zones" main parameters
                EditorGUILayout.PropertyField(useSpecialZones);
                if (EditorGUILayout.BeginFadeGroup(useSpecialZones.boolValue ? 1 : 0))
                {
                    EditorGUIUtils.PropertySliderField(serializedObject.FindProperty("safeZoneRadius"), 0, 10,
                        new GUIContent("Safe zone radius", "Radius of an optional safe zone area"));

                    EditorGUIUtils.PropertySliderField(serializedObject.FindProperty("attackRangeRadius"), 0, 10,
                        new GUIContent("Attack range radius", "Radius of an optional attack range area"));
                }
                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.Space();

                EditorGUIUtils.PropertySliderField(serializedObject.FindProperty("zoneCheckInterval"), 0.001f, 1,
                    new GUIContent("Update interval", "Time interval between zone checks (i.e. aiSensor update)"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("obstacleLayerMask"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("target"));
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region EDITOR PARAMETERS GROUP
            showFovEditorParameters = EditorGUILayout.BeginFoldoutHeaderGroup(showFovEditorParameters, "Visual Parameters");
            if (showFovEditorParameters)
            {
                EditorGUIUtils.PropertySliderField(serializedObject.FindProperty("thickness"), 0.5f, 2,
                    new GUIContent("Thickness", "Handles thickness"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("mainFovColor"));

                // shows "Special Zones" visual parameters
                if (EditorGUILayout.BeginFadeGroup(useSpecialZones.boolValue ? 1 : 0))
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("safeZoneColor"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRangeColor"));
                }
                EditorGUILayout.EndFadeGroup();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region ZONE CHECKS GROUP
            showFovChecks = EditorGUILayout.BeginFoldoutHeaderGroup(showFovChecks, "Zones Check Debug");
            if (showFovChecks)
            {
                EditorGUI.BeginDisabledGroup(true);
            
                aiSensor.targetInsideViewOuterRadius = EditorGUILayout.Toggle("Target inside outer view radius", aiSensor.targetInsideViewOuterRadius);
                aiSensor.targetSpotted = EditorGUILayout.Toggle("Target spotted", aiSensor.targetSpotted);
                
                // shows "Special Zones" debug checks
                if (EditorGUILayout.BeginFadeGroup(useSpecialZones.boolValue ? 1 : 0))
                {
                    aiSensor.targetInsideSafeZone = EditorGUILayout.Toggle("Target inside safe zone", aiSensor.targetInsideSafeZone);
                    aiSensor.targetInsideAttackRange = EditorGUILayout.Toggle("Target inside attack range", aiSensor.targetInsideAttackRange);
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

            var fovPos = aiSensor.transform.position;
            var thickness = aiSensor.thickness;

            // draws outer and inner view radius
            DrawFovZone(fovPos, 360f, aiSensor.viewOuterRadius, thickness, aiSensor.mainFovColor);
            DrawFovZone(fovPos, 360f, aiSensor.viewInnerRadius, thickness, aiSensor.mainFovColor);

            // calculates and draws view angle cone
            var viewAngleLeft = CalculateDirectionFromAngle((-aiSensor.viewAngle + 180) * 0.5f, aiSensor.transform.eulerAngles.z); // left view angle: \|
            var viewAngleRight = CalculateDirectionFromAngle((aiSensor.viewAngle + 180) * 0.5f, aiSensor.transform.eulerAngles.z); // right view angle: |/
            
            Handles.color = aiSensor.mainFovColor;
            Handles.DrawLine(fovPos, fovPos + viewAngleLeft * aiSensor.viewOuterRadius, aiSensor.thickness);
            Handles.DrawLine(fovPos, fovPos + viewAngleRight * aiSensor.viewOuterRadius, aiSensor.thickness);
            
            // draws special zones if used
            if (useSpecialZones.boolValue is true)
            {
                DrawFovZone(fovPos, 360f, aiSensor.safeZoneRadius, thickness, aiSensor.safeZoneColor); // draws safe zone radius
                DrawFovZone(fovPos, 360f, aiSensor.attackRangeRadius, thickness, aiSensor.attackRangeColor); // draws attack range radius
            }
            
            // draws line from character to spotted target
            if (aiSensor.targetInsideViewOuterRadius)
            {
                Handles.color = aiSensor.targetSpotted ? aiSensor.targetSpottedColor : aiSensor.targetHiddenColor;
                Handles.DrawLine(fovPos, aiSensor.target.transform.position, thickness);
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
        private static void DrawFovZone(Vector3 center, float angle, float radius, float lineThickness, Color color)
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