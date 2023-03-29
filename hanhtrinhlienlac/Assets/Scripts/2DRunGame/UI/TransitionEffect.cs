using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEffect : MonoBehaviour
{
    [SerializeField] Animation _effect;
    private Action _onChangeEvent;
    public void PlayEffect(Action finish)
    {
        _onChangeEvent = finish;
        _effect.Play();
        
    }
    public void OnNotifyChangeGameScene()
    {
        _onChangeEvent?.Invoke();
    }
}