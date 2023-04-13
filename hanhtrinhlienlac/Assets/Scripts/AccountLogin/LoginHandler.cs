using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoginHandler : AccountRequestBase<BasicInputFields>
{
    private static readonly string ACCOUNT_SERVICE_LOGIN_PATH = "/api/v1/pub/login";

    [Serializable] class LoginData: RequestData {}
    [Serializable] class LoginResponseData {
        public string accessToken;
        public UserInfo userInfo;
        public int[] games;
        public int totalBonusScore;
        public int totalScore;

        [Serializable] public class UserInfo {
            public string id;
            public string inviteCode;
            public string username;
        }
    }

    enum EError {
        NONE,
        NOT_ENOUGH_INFO,
        USERNAME_PASSWORD_INCORRECT,
        UNKNOWN
    }

    [SerializeField] private RegisterHandler registerHandler;
    [SerializeField] private PasswordResetHandler pwResetHandler;

    void Awake()
    {
        Show();
    }

    public void OnAnyInputValueChanged(string value)
    {
        SetErrorMsg(EError.NONE);
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
            username = inputFields.username.text,
            password = inputFields.password.text
        };

        if (!data.isValid()) {
            SetErrorMsg(EError.NOT_ENOUGH_INFO);
            return;
        }

        PostRequest(ACCOUNT_SERVICE_LOGIN_PATH, data);
    }

    public void OnRegisterBtnClicked()
    {
        Hide(() => registerHandler.Show());
    }

    public void OnPwResetBtnClicked()
    {
        Hide(() => pwResetHandler.Show());
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
                    SchoolApiSession.Inst.OnLoggedIn(responseData.accessToken);
                    UserInfo.GetInstance().OnLoggedIn(responseData.userInfo.id, responseData.userInfo.inviteCode, responseData.userInfo.username);
                }
                break;
                case 400: {
                    var responseData = JsonUtility.FromJson<ResponseData>(uwr.downloadHandler.text);
                    switch (responseData.code) {
                        case 102:
                        case 103:
                        SetErrorMsg(EError.USERNAME_PASSWORD_INCORRECT);
                        break;
                        default:
                        SetErrorMsg(EError.UNKNOWN);
                        break;
                    }
                }
                break;
                default:
                    Debug.LogError("Unhandled response code!!! " + uwr.responseCode);
                    SetErrorMsg(EError.UNKNOWN);
                    break;
            }
        } else {
            Debug.LogError("Unhandled response code!!! " + uwr.result);
            SetErrorMsg(EError.UNKNOWN);
        }
    }

    private void SetErrorMsg(EError error)
    {
        bool show = error != EError.NONE;
        errMsgTMP.gameObject.SetActive(show);
        if (show) {
            ResetTabNavSelection();
            switch (error) {
                case EError.NOT_ENOUGH_INFO: 
                errMsgTMP.text = "Vui lòng nhập đầy đủ thông tin!";
                break;
                case EError.USERNAME_PASSWORD_INCORRECT: 
                errMsgTMP.text = "Tài khoản hoặc mật khẩu không đúng.\nVui lòng kiểm tra lại!";
                break;
                default:
                errMsgTMP.text = "Unknown error!";
                break;
            }
        }
    }
}
