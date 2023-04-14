using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountResultPopup : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textRegisterSuccess;
    [SerializeField] private TMPro.TextMeshProUGUI textPwResetSuccss;
    [SerializeField] private BoundInAndOut boundInOutAnim;

    private Action onCloseCB;
    
    public void Show(bool register, bool resetPw, Action onClose)
    {
        gameObject.SetActive(true);
        textRegisterSuccess.gameObject.SetActive(register);
        textPwResetSuccss.gameObject.SetActive(resetPw);
        onCloseCB = onClose;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnOkBtnClicked()
    {
        Hide();
        onCloseCB?.Invoke();
    }

    void OnEnable()
    {
        boundInOutAnim?.PlayBoundInEffect();
    }
}
