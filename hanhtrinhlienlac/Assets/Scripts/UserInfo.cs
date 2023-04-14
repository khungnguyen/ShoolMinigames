using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserInfo
{
    private static UserInfo _inst;

    private string _id;
    public string Id { get => _id; }
    private string _inviteCode;
    public string InviteCode { get => _inviteCode; }
    private string _username;
    public string Username { get => _username; }
    private string _skinName = "";
    private bool _isPlayerCompletedRunGame = false;
    private UserInfo()
    {
        SetUnlockLevel(CheckPointType.CHECK_POINT_1);
    }
    public static UserInfo GetInstance()
    {
        if (_inst == null)
        {
            _inst = new();
        }
        return _inst;
    }

    public void OnLoggedIn(string id, string inviteCode, string username)
    {
        _id = id;
        _inviteCode = inviteCode;
        _username = username;
    }

    public void SetSkin(string s)
    {
        _skinName = s;
        SavePrefsString(Defined.SAVE_KEY_CHAR, s);
    }
    public string GetSkin()
    {
        if (_skinName.Equals(""))
        {
            _skinName = LoadPrefsString(Defined.SAVE_KEY_CHAR, "char_1");
        }
        return _skinName;
    }
    public void SetCompletedRunGame(bool b = true)
    {
        _isPlayerCompletedRunGame = true;
    }
    public bool IsPlayerCompleteRunGame()
    {
        return _isPlayerCompletedRunGame;
    }
    public void SetUnlockLevel(CheckPointType lv)
    {
        SavePrefsInt(lv.ToString(),1);
    }
    public bool IsLevelUnlocked(CheckPointType lv) {
        return LoadPrefsInt(lv.ToString(),0) == 1;
    }
    public void SavePrefsInt(string key, int s)
    {
        PlayerPrefs.SetInt(key, s);
        PlayerPrefs.Save();
    }

    public int LoadPrefsInt(string key,int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    public void SavePrefsString(string key, string s)
    {
        PlayerPrefs.SetString(key, s);
        PlayerPrefs.Save();
    }
    public string LoadPrefsString(string key, string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }
    public bool IsLastLevelUnloked() {
       return IsLevelUnlocked(CheckPointType.CHECK_POINT_3);
    }

}

