﻿using UnityEditor;
using UnityEngine;
using UtilsToolbox.PropertyAttributes;

namespace UtilsToolbox.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(PreviewAssetAttribute))]
    public class PreviewAssetPropertyDrawer : PropertyDrawer
    {
        private static class Style
        {
            internal const float HEIGHT = 18f;
            internal const float OFFSET = 6.0f;
            internal const float SPACING = 2.0f;

            internal static readonly GUIStyle TextureStyle;
            internal static readonly GUIStyle PreviewStyle;

            static Style()
            {
                TextureStyle = new GUIStyle();
                PreviewStyle = new GUIStyle("helpBox");
            }
        }
        
        private PreviewAssetAttribute Attribute => attribute as PreviewAssetAttribute;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Object target = GetValidTarget(property.objectReferenceValue);
            Texture2D previewTexture = AssetPreview.GetAssetPreview(target);

            if (previewTexture == null)
            {
                return base.GetPropertyHeight(property, label);
            }
            
            float additionalHeight = Attribute.Height + Style.OFFSET * 2 + Style.SPACING * 2;
            if (!Attribute.UseLabel)
            {
                additionalHeight -= Style.HEIGHT;
            }

            return Style.HEIGHT + additionalHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Attribute.UseLabel)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            else
            {
                position.y -= Style.HEIGHT;
            }

            Object target = GetValidTarget(property.objectReferenceValue);
            if (target)
            {
                var previewTexture = AssetPreview.GetAssetPreview(target);
                if (previewTexture == null)
                {
                    return;
                }

                DrawAssetPreview(position, previewTexture);
            }
        }

        private void DrawAssetPreview(Rect rect, Texture2D previewTexture)
        {
            float indent = rect.width - EditorGUI.IndentedRect(rect).width;

            //set image base properties
            float width = Mathf.Clamp(Attribute.Width, previewTexture.width, Attribute.Width);
            float height = Mathf.Clamp(Attribute.Height, previewTexture.height, Attribute.Height);
            Style.TextureStyle.normal.background = previewTexture;
            
            //set additional height as preview + 2x spacing + 2x frame offset
            rect.width = width + Style.OFFSET + indent;
            rect.height = height + Style.OFFSET;
            rect.y += Style.HEIGHT + Style.SPACING;
            
            //draw background frame
            EditorGUI.LabelField(rect, GUIContent.none, Style.PreviewStyle);
            rect.width = width + indent;
            rect.height = height;
            
            //adjust image to frame center
            rect.y += Style.OFFSET * 0.5f;
            rect.x += Style.OFFSET * 0.5f;
            
            //draw texture without label
            EditorGUI.LabelField(rect, GUIContent.none, Style.TextureStyle);
        }

        private Object GetValidTarget(Object referenceValue)
        {
            if (referenceValue)
            {
                return referenceValue switch
                {
                    Component component => component.gameObject,
                    _ => referenceValue
                };
            }

            return null;
        }
    }
}