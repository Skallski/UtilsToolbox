using System;
using System.IO;
using UnityEngine;

namespace SkalluUtils.Utils.Json
{
    public static class JsonDataReader
    {
        public static bool Read<T>(string jsonFilePath, ref T data)
        {
            try
            {
                StreamReader reader = new StreamReader(jsonFilePath);
                string text = reader.ReadToEnd();
                reader.Close();

                return ParseStringToObject(text, ref data);
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("JSON read error: File not found!");
                return false;
            }
        }

        public static bool Read<T>(TextAsset jsonFile, ref T data)
        {
            if (jsonFile == null)
            {
                Debug.LogError("JSON read error: File not found!");
                return false;
            }

            return ParseStringToObject(jsonFile.text, ref data);
        }

        private static bool ParseStringToObject<T>(string text, ref T data)
        {
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogError("JSON parsing error: Text is empty!");
                return false;
            }

            try
            {
                data = JsonUtility.FromJson<T>(text);
                
                Debug.Log($"<color=green>JSON parsed successfully to {data.GetType()}!</color>");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("JSON parsing error: File is not a JSON!");
                return false;
            }
        }
    }
}