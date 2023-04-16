using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardItemInfo
{
    public string gameId;
    public string userId;
    public string username;
    public int finalScore; // leaderboard for each game
    public int totalScore; // leaderboard for total 3 games
    public int rank;
    public int DisplayingScore { get => totalScore > 0 ? totalScore : finalScore; } //Support 2 cases
}
