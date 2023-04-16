using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public abstract class SchoolApiRequestBase: MonoBehaviour
{
    private static readonly string ACCOUNT_SERVICE_BASE_URL = "https://an-school-portal.securityzone.vn";

    [Serializable] public class PostData {
    }
    
    [Serializable] public class ResponseData {
        public int status;
        public int code;
        public string message;
        public string err;
    }

    abstract protected void onPostRequestCB(UnityWebRequest uwr);
    abstract protected void onGetRequestCB(UnityWebRequest uwr);

    protected void SendPostRequest(string urlPath, PostData toJsonObject, string accessToken = null)
    {
        StartCoroutine(SendPostRequestIEnumerator(urlPath, toJsonObject, accessToken));
    }

    protected void SendGetRequest(string urlPath, string accessToken = null)
    {
        StartCoroutine(SendGetRequestIEnumerator(urlPath, accessToken));
    }

    private IEnumerator SendPostRequestIEnumerator(string urlPath, PostData toJsonObject, string accessToken = null)
    {
        var url = ACCOUNT_SERVICE_BASE_URL + urlPath;

        UnityWebRequest uwr = UnityWebRequest.Post(url, "");
        if (toJsonObject != null) {
            var jsonStr = JsonUtility.ToJson(toJsonObject);
            var bytes = System.Text.Encoding.UTF8.GetBytes(jsonStr);
            var uploadHandler = new UploadHandlerRaw(bytes);
            uploadHandler.contentType = "application/json";
            uwr.uploadHandler = uploadHandler;
        }
        
        if (accessToken?.Length > 0) {
            uwr.SetRequestHeader("Authorization", "Bearer " + accessToken);
        }

        yield return uwr.SendWebRequest();

        onPostRequestCB(uwr);
    }

    private IEnumerator SendGetRequestIEnumerator(string urlPath, string accessToken = null)
    {
        var url = ACCOUNT_SERVICE_BASE_URL + urlPath;

        UnityWebRequest uwr = UnityWebRequest.Get(url);
        
        if (accessToken?.Length > 0) {
            uwr.SetRequestHeader("Authorization", "Bearer " + accessToken);
        }

        yield return uwr.SendWebRequest();

        onGetRequestCB(uwr);
    }
}
