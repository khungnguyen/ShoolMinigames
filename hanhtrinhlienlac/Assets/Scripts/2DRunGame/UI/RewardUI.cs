using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    [SerializeField] TMP_Text _score;
    [SerializeField] GameObject[] objectsDisappear;
    [SerializeField] BoundInAndOut _animation;

     [SerializeField] GameObject _okBtGameObject;
     [SerializeField] GameObject _continuesBtGameObject;

    public Action OnBackListener;
    public void SetScoreText(string s)
    {
        _score.SetText(_score.text.Replace("XXX", s));
    }
    public void Show(string scoreNumber,Action onBackListener = null, bool usOKText = false)
    {
        _okBtGameObject.SetActive(usOKText);
        _continuesBtGameObject.SetActive(!usOKText);
        SetScoreText(scoreNumber);
        gameObject.SetActive(true);
        OnBackListener = onBackListener;

        foreach (var obj in objectsDisappear) {
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup) {
                canvasGroup.alpha = 0;
            } else {
                obj.SetActive(false);
            }
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void OnBackClick() {
        OnBackListener?.Invoke();
       // Hide();
    }
    void OnEnable()
    {
        _animation?.PlayBoundInEffect();
    }
}
