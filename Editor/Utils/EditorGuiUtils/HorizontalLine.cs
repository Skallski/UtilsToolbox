using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Utils.EditorGuiUtils
{
    public static class HorizontalLine
    {
        private static readonly Color DefaultColor = new Color(0f, 0f, 0f, 0.3f);
        private static readonly float DefaultHeight = 1f;
        private static readonly Vector2 DefaultMargin = new Vector2(2f, 2f);

        public static void Create(Color color, float height, Vector2 margin)
        {
            if (color == default) color = DefaultColor;
            if (height == default) height = DefaultHeight;
            if (margin == default) margin = DefaultMargin;

            GUILayout.Space(margin.x);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, height), color);
            GUILayout.Space(margin.y);
        }
    }
}