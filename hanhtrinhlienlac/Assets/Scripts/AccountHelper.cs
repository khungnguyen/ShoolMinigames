using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

enum ERegistrationErrorCode {
    USERNAME_EXISTED = 100,
    INVITATION_CODE_INVALID = 101,
}

public class AccountHelper : MonoBehaviour
{
    [Serializable] struct RegistrationData {
        public string inviteCode;
        public string username;
        public string password;
    }
    [Serializable] struct ResetPasswordData {
        public string inviteCode;
        public string username;
        public string password;
    }

    [Serializable] struct LoginData {
        public string username;
        public string password;
    }

    [Serializable] struct ResponseData {
        public int code;
        public string message;
        public int status;
        public string err;
    }
    private static readonly string ACCOUNT_SERVICE_BASE_URL = "https://an-school-portal.securityzone.vn";
    [SerializeField] private TMPro.TMP_InputField invitationCodeTMP;
    [SerializeField] private TMPro.TMP_InputField usernameTMP;
    [SerializeField] private TMPro.TMP_InputField passwordTMP;
    [SerializeField] private TMPro.TMP_InputField passwordVerificationTMP;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBtnRegisterClicked()
    {
        StartCoroutine(PostRegistration(invitationCodeTMP.text, usernameTMP.text, passwordTMP.text, OnRegistrationCallback));
    }

    public void OnBtnResetPasswordClicked()
    {
        
    }

    public void OnBtnLoginClicked()
    {
        StartCoroutine(PostLogin(usernameTMP.text, passwordTMP.text, OnLoginCallback));
    }

    private IEnumerator PostRegistration(string invatationCode, string username, string password, Action<UnityWebRequest> onRegistrationCallback)
    {
        var url = ACCOUNT_SERVICE_BASE_URL + "/api/v1/pub/register";

        var data = new RegistrationData() {
            inviteCode = invatationCode,
            username = username,
            password = password
        };

        UnityWebRequest uwr = UnityWebRequest.Post(url, "");
        uwr.uploadHandler = CreateJsonUploadHandler(data);
        yield return uwr.SendWebRequest();

        onRegistrationCallback(uwr);
    }

    private void OnRegistrationCallback(UnityWebRequest uwr)
    {
        if (uwr.result == UnityWebRequest.Result.Success 
            || uwr.result == UnityWebRequest.Result.ProtocolError) { //Not sure why it still gets this kind of error
            Debug.Log("[OnRegistrationCallback] responseCode: " + uwr.responseCode);
            Debug.Log("[OnRegistrationCallback] downloadHandler.text: " + uwr.downloadHandler.text);
            switch (uwr.responseCode) {
                case 200:
                    Debug.Log("[OnRegistrationCallback] Register successfully!");
                    break;
                case 400:
                    var responseData = JsonUtility.FromJson<ResponseData>(uwr.downloadHandler.text);
                    switch (responseData.code) {
                        case (int) ERegistrationErrorCode.USERNAME_EXISTED:
                            Debug.LogError("Username is already existed!!");
                            break;
                        case (int) ERegistrationErrorCode.INVITATION_CODE_INVALID:
                            Debug.LogError("Invitation code is not valid!!");
                            break;
                        default:
                            Debug.LogError("Unhandled code!!! " + uwr.downloadHandler.text);
                            break;
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

    private IEnumerator PostLogin(string username, string password, Action<UnityWebRequest> onLoginCallback)
    {
        var url = ACCOUNT_SERVICE_BASE_URL + "/api/v1/pub/login";

        var data = new LoginData() {
            username = username,
            password = password
        };

        UnityWebRequest uwr = UnityWebRequest.Post(url, "");
        uwr.uploadHandler = CreateJsonUploadHandler(data);
        yield return uwr.SendWebRequest();

        onLoginCallback(uwr);
    }

    private void OnLoginCallback(UnityWebRequest uwr)
    {
        if (uwr.result == UnityWebRequest.Result.Success 
            || uwr.result == UnityWebRequest.Result.ProtocolError) { //Not sure why it still gets this kind of error
            Debug.Log("[OnRegistrationCallback] responseCode: " + uwr.responseCode);
            Debug.Log("[OnRegistrationCallback] downloadHandler.text: " + uwr.downloadHandler.text);
            switch (uwr.responseCode) {
                case 200:
                    Debug.Log("[OnRegistrationCallback] Register successfully!");
                    break;
                case 400:
                    var responseData = JsonUtility.FromJson<ResponseData>(uwr.downloadHandler.text);
                    switch (responseData.code) {
                        case (int) ERegistrationErrorCode.USERNAME_EXISTED:
                            Debug.LogError("Username is already existed!!");
                            break;
                        case (int) ERegistrationErrorCode.INVITATION_CODE_INVALID:
                            Debug.LogError("Invitation code is not valid!!");
                            break;
                        default:
                            Debug.LogError("Unhandled code!!! " + uwr.downloadHandler.text);
                            break;
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

    private UploadHandlerRaw CreateJsonUploadHandler(object obj)
    {
        var jsonStr = JsonUtility.ToJson(obj);
        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        var uploadHandler = new UploadHandlerRaw(bytes);
        uploadHandler.contentType = "application/json";
        return uploadHandler;
    }
}
