using System;
using System.IO;
using UnityEngine;

namespace SkalluUtils.Utils.Json
{
    public static class JsonDataReader
    {
        public static void Read<T>(string inputFilePath, ref T data, Action onReadComplete = null)
        {
            if (inputFilePath == string.Empty || File.Exists(inputFilePath) == false)
            {
                Debug.LogError($"File not found: {inputFilePath}");
                return;
            }

            StreamReader reader = new StreamReader(inputFilePath);
            string content = reader.ReadToEnd();
            reader.Close();

            data = JsonUtility.FromJson<T>(content);
            
            Debug.Log($"Data loaded from file {inputFilePath}");
            
            onReadComplete?.Invoke();
        }
    }
}