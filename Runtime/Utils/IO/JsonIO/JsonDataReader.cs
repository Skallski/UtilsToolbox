using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace SkalluUtils.Utils.IO.JsonIO
{
    public static class JsonDataReader
    {
        public static async Task<T> Read<T>(string inputFilePath, Action<T> onSuccess = null, Action onError = null)
        {
            try
            {
                using (StreamReader reader = new StreamReader(inputFilePath))
                {
                    string text = await reader.ReadToEndAsync();
                    T parsedData = await ParseStringToObjectAsync<T>(text);
                    
                    onSuccess?.Invoke(parsedData);
                    return parsedData;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"JSON read error: {ex.Message}");
                onError?.Invoke();
            }

            return default;
        }

        private static async Task<T> ParseStringToObjectAsync<T>(string text)
        {
            try
            {
                return await Task.FromResult(JsonUtility.FromJson<T>(text));
            }
            catch (Exception ex)
            {
                Debug.LogError($"JSON parsing error: {ex.Message}");
                return default;
            }
        }
    }
}