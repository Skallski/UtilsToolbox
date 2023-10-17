using System;
using System.IO;
using UnityEngine;

namespace SkalluUtils.Utils.Json
{
    public static class JsonDataWriter
    {
        public static void Write<T>(string outputFilePath, ref T data, Action onWriteComplete = null)
        {
            if (outputFilePath == string.Empty)
            {
                return;
            }
            
            string content = JsonUtility.ToJson(data);
            File.WriteAllText(outputFilePath, content);
            
            Debug.Log($"Data saved to file {outputFilePath}");
            
            onWriteComplete?.Invoke();
        }
    }
}