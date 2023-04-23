using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Utils
{
    /// <summary>
    /// Test object shake inside editor and during play mode
    /// </summary>
    [CustomEditor(typeof(ObjectShake))]
    public class ObjectShakeEditor : Editor
    {
        private ObjectShake _objectShake;

        private void OnEnable() => _objectShake = (ObjectShake) target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            if (GUILayout.Button("Test Shake"))
                _objectShake.Shake();
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