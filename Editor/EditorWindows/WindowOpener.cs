using System;
using UnityEditor;
using UnityEngine;

namespace UtilsToolbox.Editor.EditorWindows
{
    public static class WindowOpener
    {
        private const string MENU_NAME_BASE = "Window/Utils Toolbox/";

        [MenuItem(MENU_NAME_BASE + "Animator Controller States Display")]
        public static void OpenAnimatorControllerStatesDisplay()
        {
            OpenWindow<AnimatorControllerStatesDisplay>();
        }
        
        [MenuItem(MENU_NAME_BASE + "Components Finder")]
        public static void OpenComponentFinder()
        {
            OpenWindow<ComponentsFinder>();
        }
        
        [MenuItem(MENU_NAME_BASE + "Game Object Renamer")]
        public static void OpenGameObjectRenamer()
        {
            OpenWindow<GameObjectRenamer>();
        }
        
        [MenuItem(MENU_NAME_BASE + "Sprite Color Analyzer")]
        public static void OpenSpriteColorAnalyzer()
        {
            OpenWindow<SpriteColorAnalyzer>();
        }
        
        [MenuItem(MENU_NAME_BASE + "TextMeshPro Finder")]
        public static void OpenTmpFinder()
        {
            OpenWindow<TmpFinder>();
        }

        private static void OpenWindow<TWindow>() where TWindow : EditorWindowBase
        {
            Type type = typeof(TWindow);
            EditorWindow window = EditorWindow.GetWindow(type);
            window.titleContent = new GUIContent(type.Name);
            window.Show();
        }
    }
}