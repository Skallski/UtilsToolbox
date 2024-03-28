using System;
using System.IO;
using UnityEngine;

namespace SkalluUtils.Utils.IO.JsonIO
{
    public static class JsonDataWriter
    {
        public static async void Write<T>(string outputFilePath, T data, Action onSuccess = null, Action OnError = null)
        {
            try
            {
                string content = JsonUtility.ToJson(data, true);
                using (StreamWriter writer = new StreamWriter(outputFilePath))
                {
                    await writer.WriteAsync(content);
                    onSuccess?.Invoke();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error writing to file: {e.Message}");
                OnError?.Invoke();
            }
        }
    }
}