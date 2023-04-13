using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class RegisterInputFields: BasicInputFields {
    public TMPro.TMP_InputField pwConfirm;
    public TMPro.TMP_InputField inviteCode;
}

public class RegisterHandler : AccountRequestBase<RegisterInputFields>
{
    private static readonly string ACCOUNT_SERVICE_REGISTER_PATH = "/api/v1/pub/register";

    [Serializable] class RegisterData: RequestData {
        public string inviteCode;
    }
    [Serializable] class RegisterResponseData {
        public int code;
    }

    enum EError {
        NONE,
        NOT_ENOUGH_INFO,
        PASSWORDS_NOT_MATCHED,
        INVITE_CODE_NOT_VALID,
        INVITE_CODE_HAS_BEEN_USED,
        USERNAME_EXISTED,
        UNKNOWN
    }

    [SerializeField] private LoginHandler loginHandler;

    public void OnAnyInputValueChanged(string value)
    {
        SetErrorMsg(EError.NONE);
    }

    public void OnPasswordConfirmEndEdit(string value)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
            OnRegisterBtnClicked();
        }
    }
    
    public void OnRegisterBtnClicked()
    {
        if (inputFields.inviteCode.text.Length == 0 || inputFields.username.text.Length == 0 
            || inputFields.password.text.Length == 0 || inputFields.pwConfirm.text.Length == 0) {
            SetErrorMsg(EError.NOT_ENOUGH_INFO);
            return;
        }

        if (inputFields.password.text != inputFields.pwConfirm.text) {
            SetErrorMsg(EError.PASSWORDS_NOT_MATCHED);
            return;
        }

        var data = new RegisterData() {
            inviteCode = inputFields.inviteCode.text,
            username = inputFields.username.text,
            password = inputFields.password.text
        };

        PostRequest(ACCOUNT_SERVICE_REGISTER_PATH, data);
    }

    public void OnBackBtnClicked()
    {
        Hide(() => loginHandler.Show());
    }

    protected override void onRequestCB(UnityWebRequest uwr)
    {
        if (uwr.result == UnityWebRequest.Result.Success 
            || uwr.result == UnityWebRequest.Result.ProtocolError) { //Not sure why it still gets this kind of error
            Debug.Log("[onRequestCB] responseCode: " + uwr.responseCode);
            Debug.Log("[onRequestCB] downloadHandler.text: " + uwr.downloadHandler.text);
            switch (uwr.responseCode) {
                case 200: {
                    Debug.Log("[onRequestCB] Account registered successfully!");
                    var responseData = JsonUtility.FromJson<RegisterResponseData>(uwr.downloadHandler.text);
                }
                break;
                case 400: {
                    var responseData = JsonUtility.FromJson<ResponseData>(uwr.downloadHandler.text);
                    switch (responseData.code) {
                        case 100:
                        SetErrorMsg(EError.USERNAME_EXISTED);
                        break;
                        case 101:
                        SetErrorMsg(EError.INVITE_CODE_NOT_VALID);
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
                case EError.PASSWORDS_NOT_MATCHED:
                errMsgTMP.text = "Mật khẩu không trùng khớp.\nVui lòng kiểm tra lại!";
                break;
                case EError.INVITE_CODE_NOT_VALID:
                errMsgTMP.text = "Mã thư mời không khả dụng.\nVui lòng kiểm tra lại!";
                break;
                case EError.INVITE_CODE_HAS_BEEN_USED: 
                errMsgTMP.text = "Mã thư mời đã hết hiệu lực.\nVui lòng kiểm tra lại!";
                break;
                case EError.USERNAME_EXISTED:
                errMsgTMP.text = "Tài khoản đã được đăng ký.\nVui lòng tạo một tài khoản khác!";
                break;
                default:
                errMsgTMP.text = "Unknown error!";
                break;
            }
        }
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
