using UnityEngine;

namespace SkalluUtils.Utils.Json
{
    public class JsonFileLocator : MonoBehaviour
    {
#if UNITY_EDITOR
        [field: SerializeField, Multiline]
        [Tooltip("Path to JSON file inside the editor")]
        public string EditorFilePath { get; private set; }
#endif

        [field: SerializeField, Multiline]
        [Tooltip("Path to JSON file inside the build")]
        public string BuildFilePath { get; private set; }

        public static bool IsPathValid(string filePath)
        {
            if (filePath.Equals(string.Empty) || System.IO.File.Exists(filePath) == false)
            {
                Debug.LogError($"File not found: {filePath}");
                return false;
            }

            return true;
        }
    }
}