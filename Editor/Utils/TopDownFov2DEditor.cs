using SkalluUtils.Utils;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace SkalluUtils.Editor.Utils
{
    [CustomEditor(typeof(TopDownFov2D)), CanEditMultipleObjects]
    public class TopDownFov2DEditor : UnityEditor.Editor
    {
        private TopDownFov2D fov;

        #region GROUP WRAPPING RELATED FIELDS
        private bool showFovParameters = true; // is "FOV Parameters" foldout header group unwrapped
        private bool showFovEditorParameters = true; // is "FOV Editor Parameters" foldout header group unwrapped
        private bool showFovChecks; // is "FOV Zone Checks" foldout header group unwrapped
        private SerializedProperty useSpecialZones;
        #endregion

        private void OnEnable()
        {
            fov = (TopDownFov2D) target;
            useSpecialZones = serializedObject.FindProperty("useSpecialZones");
        }

        /// <summary>
        /// Creates slider field for serialized property
        /// </summary>
        /// <param name="property"> serialized property </param>
        /// <param name="leftValue"> min value </param>
        /// <param name="rightValue"> max value </param>
        /// <param name="label"> label (name, tooltip) </param>
        private static void PropertySliderField(SerializedProperty property, float leftValue, float rightValue, GUIContent label)
        {
            var position = EditorGUILayout.GetControlRect();
            
            label = EditorGUI.BeginProperty(position, label, property);
            
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.Slider(position, label, property.floatValue, leftValue, rightValue);
            
            if (EditorGUI.EndChangeCheck())
                property.floatValue = newValue;
            
            EditorGUI.EndProperty();
        }

        public override void OnInspectorGUI()
        {
            if (fov == null) return;
            
            // default "script" object field
            EditorGUI.BeginDisabledGroup(true); 
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);
            EditorGUI.EndDisabledGroup();
            
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            
            #region FOV PARAMETERS GROUP
            showFovParameters = EditorGUILayout.BeginFoldoutHeaderGroup(showFovParameters, "Main parameters");
            if (showFovParameters)
            {
                PropertySliderField(serializedObject.FindProperty("viewOuterRadius"), 0, 20,
                    new GUIContent("Outer view radius", "Area that determines the ability to detect target within it, provided that it is also within the viewing angle cone"));
                
                PropertySliderField(serializedObject.FindProperty("viewInnerRadius"), 0, 10,
                    new GUIContent("Inner view radius", "The minimum area that determines the ability to detect target within it"));
                
                PropertySliderField(serializedObject.FindProperty("viewAngle"), 0, 360,
                    new GUIContent("View angle", "Angle (in degrees), which determines the ability to spot objects within its area"));

                // shows "Special Zones" main parameters
                EditorGUILayout.PropertyField(useSpecialZones);
                if (EditorGUILayout.BeginFadeGroup(useSpecialZones.boolValue ? 1 : 0))
                {
                    PropertySliderField(serializedObject.FindProperty("safeZoneRadius"), 0, 10,
                        new GUIContent("Safe zone radius", "Radius of an optional safe zone area"));

                    PropertySliderField(serializedObject.FindProperty("attackRangeRadius"), 0, 10,
                        new GUIContent("Attack range radius", "Radius of an optional attack range area"));
                }
                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.Space();

                PropertySliderField(serializedObject.FindProperty("zoneCheckInterval"), 0.001f, 1,
                    new GUIContent("Update interval", "Time interval between zone checks (i.e. fov update)"));

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
                PropertySliderField(serializedObject.FindProperty("thickness"), 0.5f, 2,
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
            
                fov.targetInsideViewOuterRadius = EditorGUILayout.Toggle("Target inside outer view radius", fov.targetInsideViewOuterRadius);
                fov.targetSpotted = EditorGUILayout.Toggle("Target spotted", fov.targetSpotted);
                
                // shows "Special Zones" debug checks
                if (EditorGUILayout.BeginFadeGroup(useSpecialZones.boolValue ? 1 : 0))
                {
                    fov.targetInsideSafeZone = EditorGUILayout.Toggle("Target inside safe zone", fov.targetInsideSafeZone);
                    fov.targetInsideAttackRange = EditorGUILayout.Toggle("Target inside attack range", fov.targetInsideAttackRange);
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
            if (fov == null) return;

            var fovPos = fov.transform.position;
            var thickness = fov.thickness;

            // draws outer and inner view radius
            DrawFovZone(fovPos, 360f, fov.viewOuterRadius, thickness, fov.mainFovColor);
            DrawFovZone(fovPos, 360f, fov.viewInnerRadius, thickness, fov.mainFovColor);

            // calculates and draws view angle cone
            Vector3 viewAngleLeft = CalculateDirectionFromAngle((-fov.viewAngle + 180) * 0.5f, fov.transform.eulerAngles.z); // left view angle: \|
            Vector3 viewAngleRight = CalculateDirectionFromAngle((fov.viewAngle + 180) * 0.5f, fov.transform.eulerAngles.z); // right view angle: |/
            
            Handles.color = fov.mainFovColor;
            Handles.DrawLine(fovPos, fovPos + viewAngleLeft * fov.viewOuterRadius, fov.thickness);
            Handles.DrawLine(fovPos, fovPos + viewAngleRight * fov.viewOuterRadius, fov.thickness);
            
            // draws special zones if used
            if (useSpecialZones.boolValue is true)
            {
                DrawFovZone(fovPos, 360f, fov.safeZoneRadius, thickness, fov.safeZoneColor); // draws safe zone radius
                DrawFovZone(fovPos, 360f, fov.attackRangeRadius, thickness, fov.attackRangeColor); // draws attack range radius
            }
            
            // draws line from character to spotted target
            if (fov.targetInsideViewOuterRadius)
            {
                Handles.color = fov.targetSpotted ? fov.targetSpottedColor : fov.targetHiddenColor;
                Handles.DrawLine(fovPos, fov.target.transform.position, thickness);
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
        private void DrawFovZone(Vector3 center, float angle, float radius, float lineThickness, Color color)
        {
            Handles.color = color;
            Handles.DrawWireArc(center, Vector3.forward, Vector3.up, angle, radius, lineThickness);
        }

        /// <summary>
        /// Calculated direction from angle
        /// </summary>
        /// <param name="inputAngle"> provided angle </param>
        /// <param name="inputEulerAngleZ"> provided euler Z angle </param>
        private Vector3 CalculateDirectionFromAngle(float inputAngle, float inputEulerAngleZ)
        {
            var newAngle = inputAngle - inputEulerAngleZ;
            return new Vector3(Mathf.Sin(newAngle * Mathf.Deg2Rad), Mathf.Cos(newAngle * Mathf.Deg2Rad), 0f);
        }
    }
}