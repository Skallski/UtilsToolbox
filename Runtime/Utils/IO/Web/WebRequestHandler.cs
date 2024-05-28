using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SkalluUtils.Utils.IO.Web
{
    public static class WebRequestHandler
    {
        #region PUBLIC METHODS
        /// <summary>
        /// Creates GET request
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine will be called </param>
        /// <param name="uri"></param>
        /// <param name="onSuccess"> Action that will be called on request success </param>
        /// <param name="onError"> Action that will be called on request error </param>
        /// <typeparam name="T"> DTO </typeparam>
        public static void GetRequest<T>(MonoBehaviour caller, string uri, Action<T> onSuccess, Action onError = null)
        {
            caller.StartCoroutine(GetRequest_Coroutine(uri, onSuccess, onError));
        }
        
        /// <summary>
        /// Creates PUT request
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine will be called </param>
        /// <param name="uri"></param>
        /// <param name="data"> Class data that will be processed by request </param>
        /// <param name="onSuccess"> Action that will be called on request success </param>
        /// <param name="onError"> Action that will be called on request error </param>
        /// <typeparam name="T"> DTO </typeparam>
        public static void PutRequest<T>(MonoBehaviour caller, string uri, T data, Action onSuccess = null, Action onError = null)
        {
            caller.StartCoroutine(PutRequest_Coroutine(uri, data, onSuccess, onError));
        }
        
        /// <summary>
        /// Creates POST request
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine will be called </param>
        /// <param name="uri"></param>
        /// <param name="data"> Class data that will be processed by request </param>
        /// <param name="onSuccess"> Action that will be called on request success </param>
        /// <param name="onError"> Action that will be called on request error </param>
        /// <typeparam name="T"> DTO </typeparam>
        public static void PostRequest<T>(MonoBehaviour caller, string uri, T data, Action onSuccess = null, Action onError = null)
        {
            caller.StartCoroutine(PostRequest_Coroutine(uri, data, onSuccess, onError));
        }
        #endregion

        private static IEnumerator GetRequest_Coroutine<T>(string url, Action<T> onSuccess, Action onError = null)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string response = webRequest.downloadHandler.text;
                    Debug.Log($"<color=green>Request successful:</color> {response}");

                    try
                    {
                        T result = JsonUtility.FromJson<T>(response);
                        onSuccess?.Invoke(result);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"JSON parsing error: {e.Message}");
                        onError?.Invoke();
                    }
                }
                else
                {
                    Debug.LogError($"Request error: {webRequest.error}");
                    onError?.Invoke();
                }
            }
        }
        
        private static IEnumerator PutRequest_Coroutine<T>(string url, T data, Action onSuccess, Action onError = null)
        {
            string jsonData = JsonUtility.ToJson(data);

            using (UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPUT))
            {
                byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string response = webRequest.downloadHandler.text;
                    Debug.Log($"<color=green>Request successful:</color> {response}");
                    onSuccess?.Invoke();
                }
                else
                {
                    Debug.LogError($"Request error: {webRequest.error}");
                    onError?.Invoke();
                }
            }
        }

        private static IEnumerator PostRequest_Coroutine<T>(string url, T data, Action onSuccess, Action onError = null)
        {
            string jsonData = JsonUtility.ToJson(data);
            
            using (UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
            {
                byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string response = webRequest.downloadHandler.text;
                    Debug.Log($"<color=green>Request successful:</color> {response}");
                    onSuccess?.Invoke();
                }
                else
                {
                    Debug.LogError($"Request error: {webRequest.error}");
                    onError?.Invoke();
                }
            }
        }
    }
}