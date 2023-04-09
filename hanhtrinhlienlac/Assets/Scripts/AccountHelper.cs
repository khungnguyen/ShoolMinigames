using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AccountHelper : MonoBehaviour
{
    private static readonly string ACCOUNT_SERVICE_URL = "https://an-school-portal.securityzone.vn";
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
        
    }

    private IEnumerator PostRegistration(string invatationCode, string username, string password, Action<UnityWebRequest> onRegistrationCallback)
    {
        var url = ACCOUNT_SERVICE_URL + "/api/v1/pub/register";
        Debug.Log("[PostRegistration] url: " + url);
        Debug.Log("[PostRegistration] invatationCode: " + invatationCode);
        Debug.Log("[PostRegistration] username: " + username);
        Debug.Log("[PostRegistration] password: " + password);

        WWWForm form = new WWWForm();
        form.AddField("inviteCode", invatationCode);
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();

        onRegistrationCallback(uwr);
    }

    private void OnRegistrationCallback(UnityWebRequest uwr)
    {
        Debug.Log("[OnRegistrationCallback] result: " + uwr.result);
        if (true || uwr.result == UnityWebRequest.Result.Success) {
            Debug.Log("[OnRegistrationCallback] responseCode: " + uwr.responseCode);
            Debug.Log("[OnRegistrationCallback] downloadHandler.text: " + uwr.downloadHandler.text);
            switch (uwr.responseCode) {
                case 200:
                    Debug.Log("[OnRegistrationCallback] Register successfully!");
                    break;
                case 400:
                    Debug.Log("[OnRegistrationCallback] Register failed!");
                    break;
            }
        } else {
            // Errors happen
        }
    }
}
