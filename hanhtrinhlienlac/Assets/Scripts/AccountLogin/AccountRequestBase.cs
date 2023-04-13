using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public abstract class AccountRequestBase : MonoBehaviour
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

    [SerializeField] protected TMPro.TMP_InputField usernameTMP;
    [SerializeField] protected TMPro.TMP_InputField passwordTMP;
    [SerializeField] private List<GameObject> tabNavObjects = new List<GameObject>();
    [SerializeField] private bool autoSelectThe1stNavObj = true;

    private int curNavObjIndex = -1;

    void Start()
    {
        if (autoSelectThe1stNavObj) {
            ResetTabNavSelection();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            curNavObjIndex += shift ? -1 : 1;
            curNavObjIndex %= tabNavObjects.Count;
            SelectNavObjIndex(curNavObjIndex);
        }
    }

    public void ResetTabNavSelection()
    {
        curNavObjIndex = 0;
        SelectNavObjIndex(curNavObjIndex);
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

    private void SelectNavObjIndex(int index)
    {
        if (index < 0 || index >= tabNavObjects.Count) {
            return;
        }
        var obj = tabNavObjects[index];
        EventSystem.current.SetSelectedGameObject(obj, new BaseEventData(EventSystem.current));
        obj.GetComponent<InputField>()?.OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
