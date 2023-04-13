using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolApiSession
{
    private static SchoolApiSession _inst;
    public static SchoolApiSession Inst {
        get {
            if (_inst == null) {
                _inst = new();
            }
            return _inst;
            }
    }
    private SchoolApiSession() {}

    private bool isLoggedIn = false;
    public bool IsLoggedIn { get => isLoggedIn; }
    private string accessToken;
    public string AccessToken { get => accessToken; }
    
    public void OnLoggedIn(string _accessToken)
    {
        isLoggedIn = true;
        accessToken = _accessToken;
    }
}
