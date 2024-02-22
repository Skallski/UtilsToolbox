using System;
using UnityEngine;

namespace SkalluUtils.Utils.DataBridge.ImageFileIO
{
    public static class ImageFileSaver
    {
        public enum ImageOutputEncoding
        {
            PNG,
            JPG
        }
        
        public static void Save(this Sprite sprite, string outputPath, ImageOutputEncoding encoding)
        {
            try
            {
                byte[] bytes = encoding switch
                {
                    ImageOutputEncoding.PNG => sprite.texture.EncodeToPNG(),
                    ImageOutputEncoding.JPG => sprite.texture.EncodeToJPG(),
                    _ => sprite.texture.EncodeToPNG()
                };

                System.IO.File.WriteAllBytesAsync(outputPath, bytes);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error saving Sprite to PNG: {ex}");
            }
        }
    }
}