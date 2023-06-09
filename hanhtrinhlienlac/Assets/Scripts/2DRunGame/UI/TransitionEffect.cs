using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEffect : MonoBehaviour
{
    [SerializeField] Animation _effect;
    private Action _onChangeEvent;
    public void PlayEffect(bool isEnterGame,Action finish)
    {
        _onChangeEvent = finish;
        _effect.Play(isEnterGame?"EnterGameTransition":"MinGameSceneTransition");
        
    }
    public void OnNotifyChangeGameScene()
    {
        _onChangeEvent?.Invoke();
    }
}