using System;
using System.IO;
using UnityEngine;

namespace SkalluUtils.Utils.DataBridge.ImageFileIO
{
    public static class ImageFileLoader
    {
        public static Sprite LoadAsSprite(string inputPath)
        {
            Texture2D texture = LoadAsTexture2D(inputPath);
            
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
        
        public static Texture2D LoadAsTexture2D(string inputPath)
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