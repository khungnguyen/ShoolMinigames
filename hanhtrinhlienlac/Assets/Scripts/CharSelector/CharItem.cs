using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharItem : MonoBehaviour
{
    [SerializeField] Image _avatar;
    [SerializeField] TMP_Text _name;

    private CharInfo _info;
    private Action<string> _listener;
    public void OnClick()
    {
        if (_listener != null)
        {
            _listener.Invoke(_info.charId);
        }
    }
    public CharItem SetInfo(CharInfo s)
    {
        _info = s;
        _name.SetText(_info.charName);
        return this;
    }
    public void SetClickListener(Action<string> ac)
    {
        _listener = ac;
    }
}
