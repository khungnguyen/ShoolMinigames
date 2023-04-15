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
    private List<LoginHandler.LoginResponseData.GameScore> _gameScores;
    private UserInfo()
    {
        SetUnlockLevel(CheckPointType.CHECK_POINT_1, true);
        SetUnlockLevel(CheckPointType.CHECK_POINT_2, false);
        SetUnlockLevel(CheckPointType.CHECK_POINT_3, false);
    }
    public static UserInfo GetInstance()
    {
        if (_inst == null)
        {
            _inst = new();
        }

        return _inst;
    }

    public void OnLoggedIn(LoginHandler.LoginResponseData.UserInfo userInfo, LoginHandler.LoginResponseData.GameScore[] gameScores)
    {
        _id = userInfo.id;
        _inviteCode = userInfo.inviteCode;
        _username = userInfo.username;
        foreach (var k in gameScores)
        {
            Debug.Log("k.gameId" + k.gameId + "----" + k.finalScore);
            switch (k.gameId)
            {
                case "game1":
                    {
                        SetUnlockLevel(CheckPointType.CHECK_POINT_1, true);
                        SetUnlockLevel(CheckPointType.CHECK_POINT_2, k.finalScore > 0);
                        break;
                    }

                case "game2":
                    {
                        SetUnlockLevel(CheckPointType.CHECK_POINT_3, k.finalScore > 0);
                        break;
                    }

                case "game3":
                    {
                        SetUnlockLevel(CheckPointType.CHECK_POINT_4, k.finalScore > 0);
                        break;
                    }

            }
        }
      //  _gameScores = new(gameScores);
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
    public void SetUnlockLevel(CheckPointType lv, bool unlock)
    {
        SavePrefsInt(lv.ToString(), unlock ? 1 : 0);
    }
    public bool IsLevelUnlocked(CheckPointType lv)
    {
        return LoadPrefsInt(lv.ToString(), 0) == 1;
    }
    public void SavePrefsInt(string key, int s)
    {
        PlayerPrefs.SetInt(key, s);
        PlayerPrefs.Save();
    }

    public int LoadPrefsInt(string key, int defaultValue)
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
    public bool IsLastLevelUnlocked()
    {
        return IsLevelUnlocked(CheckPointType.CHECK_POINT_3);
    }

}

