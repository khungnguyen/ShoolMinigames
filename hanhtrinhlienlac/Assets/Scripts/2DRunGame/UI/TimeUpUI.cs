using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUpUI : MonoBehaviour
{
    [SerializeField] TMP_Text _score;
    [SerializeField] GameObject[] objectsDisappear;
    [SerializeField] BoundInAndOut _animation;


    public void OnBackClick() {
        string pre = UserInfo.GetInstance().GetPreviousScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(pre);

    }
    void OnEnable()
    {
        _animation?.PlayBoundInEffect();
    }
}
