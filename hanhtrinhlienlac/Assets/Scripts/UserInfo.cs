using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserInfo
{
    private static UserInfo _inst;

    private string _skinName = "char_5";
    private UserInfo()
    {

    }
    public static UserInfo GetInstance()
    {
        if (_inst == null)
        {
            _inst = new ();
        }
        return _inst;
    }
    public void SetSkin(string s) {
        _skinName = s;
    }
    public string GetSkin() {
        return _skinName;
    }

}
