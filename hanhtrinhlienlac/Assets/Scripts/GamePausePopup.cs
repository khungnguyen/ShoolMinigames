using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GamePausePopup : MonoBehaviour
{
    private Action onResumeCB;

    public void RegisterOnResumeCB(Action cb)
    {
        onResumeCB += cb;
    }

    public void OnResumeBtnClicked()
    {
        Hide();
        onResumeCB.Invoke();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
