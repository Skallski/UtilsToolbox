using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SkalluUtils.Utils.IO.JsonIO
{
    public static class JsonOverHttpHandler
    {
        #region PUBLIC METHODS
        public static void GetRequest<T>(MonoBehaviour caller, string uri, Func<string, T> deserializeFunc,
            Action<T> onSuccess, Action onError = null)
        {
            caller.StartCoroutine(GetRequest_Coroutine(uri, deserializeFunc, onSuccess, onError));
        }
        
        public static void GetRequest<T>(MonoBehaviour caller, string uri, Action<T> onSuccess, Action onError = null)
        {
            GetRequest(caller, uri, JsonUtility.FromJson<T>, onSuccess, onError);
        }

        public static void PutRequest<T>(MonoBehaviour caller, string uri, T data, Func<T, string> serializeFunc,
            Action onSuccess = null, Action onError = null)
        {
            caller.StartCoroutine(PutRequest_Coroutine(uri, data, serializeFunc, onSuccess, onError));
        }
        
        public static void PutRequest<T>(MonoBehaviour caller, string uri, T data, Action onSuccess = null, Action onError = null)
        {
            PutRequest(caller, uri, data, d => JsonUtility.ToJson(d), onSuccess, onError);
        }

        public static void PostRequest<T>(MonoBehaviour caller, string uri, T data, Func<T, string> serializeFunc,
            Action onSuccess = null, Action onError = null)
        {
            caller.StartCoroutine(PostRequest_Coroutine(uri, data, serializeFunc, onSuccess, onError));
        }
        
        public static void PostRequest<T>(MonoBehaviour caller, string uri, T data, Action onSuccess = null, Action onError = null)
        {
            PostRequest(caller, uri, data, d => JsonUtility.ToJson(d), onSuccess, onError);
        }
        #endregion

        private static IEnumerator GetRequest_Coroutine<T>(string url, Func<string, T> deserializeFunc,
            Action<T> onSuccess, Action onError = null)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return SendRequest_Coroutine(webRequest, response =>
                {
                    try
                    {
                        T result = deserializeFunc(response);
                        onSuccess?.Invoke(result);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"JSON parsing error: {e.Message}");
                        onError?.Invoke();
                    }
                }, onError);
            }
        }

        private static IEnumerator PutRequest_Coroutine<T>(string url, T data, Func<T, string> serializeFunc,
            Action onSuccess, Action onError = null)
        {
            string jsonData = serializeFunc(data);
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);

            using (UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPUT))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                yield return SendRequest_Coroutine(webRequest, _ => onSuccess?.Invoke(), onError);
            }
        }

        private static IEnumerator PostRequest_Coroutine<T>(string url, T data, Func<T, string> serializeFunc,
            Action onSuccess, Action onError = null)
        {
            string jsonData = serializeFunc(data);
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);

            using (UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                yield return SendRequest_Coroutine(webRequest, _ => onSuccess?.Invoke(), onError);
            }
        }

        private static IEnumerator SendRequest_Coroutine(UnityWebRequest webRequest, Action<string> onSuccess, Action onError)
        {
            webRequest.SendWebRequest();
            
            // request timeout handling
            const float timeoutSeconds = 10f;
            float startTime = Time.time;
            while (webRequest.isDone == false)
            {
                if (Time.time - startTime >= timeoutSeconds)
                {
                    webRequest.Abort();
                    Debug.LogError("Request timed out!");
                    onError?.Invoke();
                    webRequest.Dispose();
                    
                    yield break;
                }

                yield return null;
            }
            
            // request completed
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text ?? string.Empty;
                Debug.Log($"<color=green>Request successful:</color> {response}");
                onSuccess?.Invoke(response);
            }
            else
            {
                Debug.LogError($"Request error: {webRequest.error}");
                onError?.Invoke();
            }
            
            webRequest.Dispose();
        }
    }
}