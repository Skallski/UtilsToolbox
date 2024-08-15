using System;
using UnityEngine;

namespace UtilsToolbox.Utils.IO.ImageIO
{
    public static class ImageWriter
    {
        public static void WriteToPNG(Sprite sprite, string outputPath)
        {
            WriteToFileInternal(sprite.texture.EncodeToPNG(), outputPath);
        }
        
        public static void WriteToJPG(Sprite sprite, string outputPath)
        {
            WriteToFileInternal(sprite.texture.EncodeToJPG(), outputPath);
        }

        private static void WriteToFileInternal(byte[] encodedSprite, string outputPath)
        {
            try
            {
                System.IO.File.WriteAllBytesAsync(outputPath, encodedSprite);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving Sprite as file: {e}");
            }
        }
    }
}