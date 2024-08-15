using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UtilsToolbox.Editor.EditorWindows
{
    public class SpriteColorAnalyzer : EditorWindowBase
    {
        private Sprite _sprite;
        private readonly Dictionary<Color, string> _colorHexMap = new Dictionary<Color, string>();
        private int _colorCounter;
        private int _maxDisplayedColors = 100;
        private Vector2 _maxResolution = new Vector2(128, 128);

        protected override void SetSize()
        {
            
        }

        private void OnGUI()
        {
            if (_sprite is null)
            {
                EditorGUILayout.HelpBox("Place sprite in the box to display it's colors.", MessageType.Info);
            }

            EditorGUI.BeginChangeCheck();
            _sprite = (Sprite) EditorGUILayout.ObjectField(GUIContent.none, _sprite, typeof(Sprite), false);
            if (EditorGUI.EndChangeCheck())
            {
                Clear();
            }

            if (_sprite is null)
            {
                return;
            }

            if (IsReadWriteOn(_sprite) == false)
            {
                Clear();
                EditorGUILayout.HelpBox($"{_sprite.name} Read Write permission cannot be false!", 
                    MessageType.Error);

                if (GUILayout.Button("Enable Read/Write"))
                {
                    EnableReadWrite(_sprite);
                }

                return;
            }
                
            _maxResolution = EditorGUILayout.Vector2Field("Max Sprite Resolution", _maxResolution);
            Rect spriteRect = _sprite.rect;
            if (spriteRect.width > _maxResolution.x || spriteRect.height > _maxResolution.y)
            {
                Clear();
                EditorGUILayout.HelpBox("Selected sprite is bigger than max supported resolution! Increase the supported resolution.", 
                    MessageType.Error);
                    
                return;
            }
                
            EditorGUI.BeginChangeCheck();
            _maxDisplayedColors = EditorGUILayout.IntField("Max Displayed Colors", _maxDisplayedColors);
            if (EditorGUI.EndChangeCheck())
            {
                if (_maxDisplayedColors < 1)
                {
                    _maxDisplayedColors = 1;
                }
                
                Clear();
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Analyze Colors"))
            {
                AnalyzeColors();
            }
                
            // display colors
            if (_colorCounter >= _maxDisplayedColors)
            {
                EditorGUILayout.HelpBox("All colors won't be displayed due to the limit of max displayed colors! Increase the limit.", 
                    MessageType.Warning);
            }

            foreach (KeyValuePair<Color, string> kvp in _colorHexMap)
            {
                GUILayout.BeginHorizontal();

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ColorField(GUIContent.none, kvp.Key, false, false, false, 
                    GUILayout.Width(50), GUILayout.Height(20));
                EditorGUI.EndDisabledGroup();

                GUILayout.Label(kvp.Value);
                GUILayout.EndHorizontal();
            }
        }

        private static void EnableReadWrite(Sprite sprite)
        {
            string spritePath = AssetDatabase.GetAssetPath(sprite.texture);
            TextureImporter textureImporter = AssetImporter.GetAtPath(spritePath) as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.isReadable = true;
                AssetDatabase.ImportAsset(spritePath, ImportAssetOptions.ForceUpdate);
            }
        }

        private static bool IsReadWriteOn(Sprite sprite)
        {
            if (sprite is null)
            {
                return false;
            }

            string path = AssetDatabase.GetAssetPath(sprite.texture);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            return textureImporter is not null && textureImporter.isReadable;
        }

        private void Clear()
        {
            _colorCounter = 0;
            _colorHexMap.Clear();
        }

        private void AnalyzeColors()
        {
            Clear();

            Texture2D texture = _sprite.texture;
            Color[] pixels = texture.GetPixels(
                (int) _sprite.textureRect.x, 
                (int) _sprite.textureRect.y, 
                (int) _sprite.textureRect.width,
                (int) _sprite.textureRect.height);

            foreach (Color pixel in pixels)
            {
                if (pixel.a > 0 && _colorHexMap.ContainsKey(pixel) == false && _colorCounter < _maxDisplayedColors)
                {
                    _colorHexMap[pixel] = ColorUtility.ToHtmlStringRGBA(pixel);
                    _colorCounter++;
                }
            }
        }
    }
}