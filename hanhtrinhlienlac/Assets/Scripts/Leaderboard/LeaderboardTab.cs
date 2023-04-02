using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardTab : MonoBehaviour
{
    private static readonly float DATA_REFRESH_INTERVAL = 300f; // 5 minutes
    [SerializeField] private ELeaderboardId id;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private TMPro.TextMeshProUGUI textTMP;

    public ELeaderboardId Id { get => id; }
    private LeaderboardData data;
    public LeaderboardData Data { get => data; }
    private float timeSinceLastLoadingData = DATA_REFRESH_INTERVAL;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastLoadingData += Time.deltaTime;
        TryGettingData();
    }

    public void TryGettingData(bool force = false)
    {
        if (!force && timeSinceLastLoadingData < DATA_REFRESH_INTERVAL) {
            return;
        }

        string path = Id == ELeaderboardId.FINAL ? "Test_LeaderboardDataFinal" : Id == ELeaderboardId.QUESTION_GAME ? "Test_LeaderboardDataQuestionGame" :
                        Id == ELeaderboardId.MEMORY_GAME ? "Test_LeaderboardDataMemoryGame" : "Test_LeaderboardDataActionGame";
        var json = Resources.Load<TextAsset>(path);
        data = JsonUtility.FromJson<LeaderboardData>(json.text);
        timeSinceLastLoadingData = 0f;

        Debug.Log("Leaderboard id: " + data.LeaderboardId);
        Debug.Log("Leaderboard name: " + data.name);
        Debug.Log("Leaderboard list count: " + data.list.Count);
    }

    public void OnSelected()
    {
        TryGettingData();
        LeaderboardsMgr.Inst.OnTabSelected(this);
        layoutElement.minWidth = 200;
        layoutElement.minHeight = 100;
        GetComponent<Image>().color = Color.green;
        textTMP.fontStyle = TMPro.FontStyles.Bold;
    }

    public void OnDeselected()
    {
        layoutElement.minWidth = 0;
        layoutElement.minHeight = 0;
        GetComponent<Image>().color = Color.white;
        textTMP.fontStyle = TMPro.FontStyles.Normal;
    }
}