using System;
using UnityEngine;

namespace SkalluUtils.Extensions
{
    public static class Texture2DExtensions
    {
        public static Sprite Texture2DToSprite(this Texture2D texture, Vector2 pivot)
        {
            try
            {
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error converting Texture2D to Sprite: {ex}");
                return null;
            }
        }
        
        public static Sprite Texture2DToSprite(this Texture2D texture)
        {
            return Texture2DToSprite(texture, Vector2.one * 0.5f);
        }
    }
}