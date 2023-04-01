using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardData
{
    public string name;
    public List<LeaderboardItemInfo> list;

    public ELeaderboardId LeaderboardId {
        get {
            return name == "Final" ? ELeaderboardId.FINAL : name == "Question" ? ELeaderboardId.QUESTION_GAME : name == "Memory" ? ELeaderboardId.MEMORY_GAME : ELeaderboardId.ACTION_GAME;
        }
    }
}
