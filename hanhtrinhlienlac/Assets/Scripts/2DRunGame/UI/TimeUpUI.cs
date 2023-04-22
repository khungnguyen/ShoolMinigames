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
    [SerializeField] SoundManager _soundManager;


    public void OnBackClick()
    {
        _soundManager.PlaySfx(_soundManager.soundData[1]);
        string pre = UserInfo.GetInstance().GetPreviousScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(pre);

    }
    void Start()
    {
        _soundManager.PlaySfx(_soundManager.soundData[0],false,-1,1,1);
    }
    void OnEnable()
    {
        _animation?.PlayBoundInEffect();
    }
}
