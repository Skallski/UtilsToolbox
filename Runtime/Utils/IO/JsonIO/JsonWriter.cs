using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace SkalluUtils.Utils.IO.JsonIO
{
    public static class JsonWriter
    {
        public static void Write<T>(string outputFilePath, T data, Func<T, string> serializeFunc, 
            Action onSuccess = null, Action onError = null)
        {
            try
            {
                string content = serializeFunc(data);
                using (StreamWriter writer = new StreamWriter(outputFilePath))
                {
                    writer.Write(content);
                    writer.Flush();
                    onSuccess?.Invoke();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error writing to file: {e.Message}");
                onError?.Invoke();
            }
        }
        
        public static void Write<T>(string outputFilePath, T data, Action onSuccess = null, Action onError = null)
        {
            Write(outputFilePath, data, d => JsonUtility.ToJson(d, true), onSuccess, onError);
        }
    }
}