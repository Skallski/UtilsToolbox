﻿using UnityEditor;
using UnityEngine;
using UtilsToolbox.Utils.VFX;

namespace UtilsToolbox.Editor.CustomEditors
{
    /// <summary>
    /// Test object shake during play mode
    /// </summary>
    [CustomEditor(typeof(ObjectShake))]
    public class ObjectShakeEditor : UnityEditor.Editor
    {
        private ObjectShake _objectShake;

        private void OnEnable() => _objectShake = target as ObjectShake;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                EditorGUILayout.Space();

                if (GUILayout.Button("Test Shake"))
                {
                    _objectShake.Shake();
                }
            }
        }

        // private void OnSceneGUI()
        // {
        //     if (!Application.isPlaying)
        //     {
        //         EditorApplication.QueuePlayerLoopUpdate();
        //         SceneView.RepaintAll();
        //     }
        // }
    }
}