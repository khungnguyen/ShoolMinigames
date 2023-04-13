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

    [Serializable] public class RequestData {
        public string username;
        public string password;
        public bool isValid() {
            return username?.Length > 0 && password?.Length > 0;
        }
    }
    [Serializable] public class ResponseData {
        public int status;
        public int code;
        public string message;
        public string err;
    }

    abstract protected void onRequestCB(UnityWebRequest uwr);

    protected void PostRequest(string urlPath, RequestData toJsonObject)
    {
        StartCoroutine(PostRequestIEnumerator(urlPath, toJsonObject));
    }

    private IEnumerator PostRequestIEnumerator(string urlPath, RequestData toJsonObject)
    {
        var url = ACCOUNT_SERVICE_BASE_URL + urlPath;

        UnityWebRequest uwr = UnityWebRequest.Post(url, "");
        var jsonStr = JsonUtility.ToJson(toJsonObject);
        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        var uploadHandler = new UploadHandlerRaw(bytes);
        uploadHandler.contentType = "application/json";
        uwr.uploadHandler = uploadHandler;

        yield return uwr.SendWebRequest();

        onRequestCB(uwr);
    }
}
