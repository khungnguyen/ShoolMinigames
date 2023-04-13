using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoginHandler : AccountRequestBase
{
    private static readonly string ACCOUNT_SERVICE_LOGIN_PATH = "/api/v1/pub/login";

    [Serializable] class LoginData: RequestData {}
    [Serializable] class LoginResponseData {
        public int code;
    }

    [SerializeField] private GameObject errMsgObj;

    public void OnUsernameInputValueChanged(string value)
    {
        SetErrorShowHide(false);
    }

    public void OnPasswordInputValueChanged(string value)
    {
        SetErrorShowHide(false);
    }

    public void OnPasswordEndEdit(string value)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
            OnLoginBtnClicked();
        }
    }
    
    public void OnLoginBtnClicked()
    {
        var data = new LoginData() {
            username = usernameTMP.text,
            password = passwordTMP.text
        };

        if (!data.isValid()) {
            Debug.LogWarning("Username or password is not valid!!");
            SetErrorShowHide(true);
            return;
        }

        PostRequest(ACCOUNT_SERVICE_LOGIN_PATH, data);
    }

    protected override void onRequestCB(UnityWebRequest uwr)
    {
        if (uwr.result == UnityWebRequest.Result.Success 
            || uwr.result == UnityWebRequest.Result.ProtocolError) { //Not sure why it still gets this kind of error
            Debug.Log("[onRequestCB] responseCode: " + uwr.responseCode);
            Debug.Log("[onRequestCB] downloadHandler.text: " + uwr.downloadHandler.text);
            switch (uwr.responseCode) {
                case 200: {
                    Debug.Log("[onRequestCB] Login successfully!");
                    var responseData = JsonUtility.FromJson<LoginResponseData>(uwr.downloadHandler.text);
                }
                break;
                case 400: {
                    var responseData = JsonUtility.FromJson<ResponseData>(uwr.downloadHandler.text);
                    switch (responseData.code) {
                        case 102:
                        case 103:
                        SetErrorShowHide(true);
                        break;
                        default:
                        Debug.LogError("Unhandled code!!! " + uwr.downloadHandler.text);
                        break;
                    }
                }
                break;
                default:
                    Debug.LogError("Unhandled response code!!! " + uwr.responseCode);
                    break;
            }
        } else {
            // Errors happen
        }
    }

    private void SetErrorShowHide(bool show)
    {
        errMsgObj.SetActive(show);
        if (show) {
            ResetTabNavSelection();
        }
    }
}
