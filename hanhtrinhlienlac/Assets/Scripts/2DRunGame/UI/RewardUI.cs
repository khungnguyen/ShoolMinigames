using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    [SerializeField] TMP_Text _score;

    public Action OnBackListener;
    public void SetScoreText(string s)
    {
        _score.SetText(_score.text.Replace("XXX", s));
    }
    public void Show(string scoreNumber,Action onBackListener = null)
    {
        SetScoreText(scoreNumber);
        gameObject.SetActive(true);
        OnBackListener = onBackListener;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void OnBackClick() {
        OnBackListener?.Invoke();
       // Hide();
    }
}
