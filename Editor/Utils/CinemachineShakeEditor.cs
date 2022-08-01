using SkalluUtils.Utils;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Editor.Utils
{
    /// <summary>
    /// Test screen shake during play mode
    /// </summary>
    [CustomEditor(typeof(CinemachineShake))]
    public class CinemachineShakeEditor : UnityEditor.Editor
    {
        private CinemachineShake cinemachineShake;

        private void OnEnable() => cinemachineShake = (CinemachineShake) target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                EditorGUILayout.Space();

                if (GUILayout.Button("Test Shake"))
                    cinemachineShake.ShakeCamera(cinemachineShake.ShakeMagnitude, cinemachineShake.ShakeTime);
            }
        }
    }
}