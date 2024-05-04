using SkalluUtils.Utils;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Editor.CustomEditors
{
    [CustomPropertyDrawer(typeof(Range))]
    public class RangeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                Rect rect = EditorGUI.PrefixLabel(position, label);
                Rect[] splittedRect = SplitRect(rect, 3);

                SerializedProperty minProp = property.FindPropertyRelative("min");
                SerializedProperty maxProp = property.FindPropertyRelative("max");
                SerializedProperty valueLeftProp = property.FindPropertyRelative("_valueLeft");
                SerializedProperty valueRightProp = property.FindPropertyRelative("_valueRight");

                EditorGUI.BeginChangeCheck();
                float minLimit = minProp.floatValue;
                float maxLimit = maxProp.floatValue;
                float minValue = valueLeftProp.floatValue;
                float maxValue = valueRightProp.floatValue;

                minValue = EditorGUI.FloatField(splittedRect[0], float.Parse(minValue.ToString("F2")));
                maxValue = EditorGUI.FloatField(splittedRect[2], float.Parse(maxValue.ToString("F2")));
                EditorGUI.MinMaxSlider(splittedRect[1], ref minValue, ref maxValue, minLimit, maxLimit);

                if (minValue < minLimit) minValue = minLimit;
                if (maxValue > maxLimit) maxValue = maxLimit;

                if (EditorGUI.EndChangeCheck())
                {
                    valueLeftProp.floatValue = minValue;
                    valueRightProp.floatValue = maxValue;
                }
            }
            EditorGUI.EndProperty();
        }
        
        private Rect[] SplitRect(Rect rectToSplit, int n)
        {
            Rect[] rects = new Rect[n];

            const float padding = 20;
            const float space = 5;
            float width = (rectToSplit.width - (2 * padding) - (2 * space)) / n;

            for (int i = 0; i < n; i++)
            {
                rects[i] = new Rect(rectToSplit.x + padding + (i * (width + space)), rectToSplit.y, width,
                    rectToSplit.height);
            }

            return rects;
        }
    }
}