using System;
using System.IO;
using UnityEngine;

namespace UtilsToolbox.Utils.IO.JsonIO
{
    public static class JsonReader
    {
        public static void Read<T>(string inputFilePath, Func<string, T> deserializeFunc, Action<T> onSuccess,
            Action onError = null)
        {
            try
            {
                using (StreamReader reader = new StreamReader(inputFilePath))
                {
                    string text = reader.ReadToEnd();
                    T parsedData = deserializeFunc(text);

                    onSuccess?.Invoke(parsedData);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"JSON read error: {ex.Message}");
                onError?.Invoke();
            }
        }
        
        public static void Read<T>(string inputFilePath, Action<T> onSuccess, Action onError = null)
        {
            Read(inputFilePath, JsonUtility.FromJson<T>, onSuccess, onError);
        }
    }
}