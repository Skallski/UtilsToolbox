using System;
using System.IO;
using UnityEngine;

namespace UtilsToolbox.Utils.IO.ImageIO
{
    public static class ImageReader
    {
        public static Sprite ReadAsSprite(string inputPath)
        {
            Texture2D texture = ReadAsTexture2D(inputPath);
            
            try
            {
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error while creating Sprite: {ex.Message}");
                return null; 
            }
        }
        
        public static Texture2D ReadAsTexture2D(string inputPath)
        {
            try
            {
                byte[] fileData = File.ReadAllBytes(inputPath);
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(fileData);
                return texture;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error while loading Texture2D: {ex.Message}");
                return null; 
            }
        }
    }
}