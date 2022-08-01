using SkalluUtils.Utils;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Editor.Utils
{
    /// <summary>
    /// Test object shake inside editor and during play mode
    /// </summary>
    [CustomEditor(typeof(ObjectShake))]
    public class ObjectShakeEditor : UnityEditor.Editor
    {
        private ObjectShake objectShake;

        private void OnEnable() => objectShake = (ObjectShake) target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            if (GUILayout.Button("Test Shake"))
                objectShake.Shake();
        }

        private void OnSceneGUI()
        {
            if (!Application.isPlaying)
            {
                EditorApplication.QueuePlayerLoopUpdate();
                SceneView.RepaintAll();
            }
        }
    }
}