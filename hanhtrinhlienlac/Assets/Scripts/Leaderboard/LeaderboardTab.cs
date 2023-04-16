using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderboardTab : SchoolApiRequestBase
{
    private static readonly string ACCOUNT_SERVICE_LB_PATH = "/api/v1/users/leaderboard/";
    private static readonly float DATA_REFRESH_INTERVAL = 300f; // 5 minutes
    [SerializeField] private ELeaderboardId id;
    [SerializeField] private TMPro.TextMeshProUGUI textTMP;
    [SerializeField] private GameObject darkLayer;

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

        string path = Id == ELeaderboardId.QUESTION_GAME ? "game1" : Id == ELeaderboardId.MEMORY_GAME ? "game2" : Id == ELeaderboardId.ACTION_GAME ? "game3" : "leaderboards";
        Debug.Log("Fetching leaderboard data for " + path);
        SendGetRequest(ACCOUNT_SERVICE_LB_PATH + path, SchoolApiSession.Inst.AccessToken);
        timeSinceLastLoadingData = 0f;
    }

    public void OnSelected()
    {
        TryGettingData();
        LeaderboardsMgr.Inst.OnTabSelected(this);
        textTMP.fontStyle = TMPro.FontStyles.Bold;
        darkLayer.SetActive(false);
    }

    public void OnDeselected()
    {
        textTMP.fontStyle = TMPro.FontStyles.Normal;
        darkLayer.SetActive(true);
    }

    protected override void onPostRequestCB(UnityWebRequest uwr)
    {
        throw new System.NotImplementedException();
    }

    protected override void onGetRequestCB(UnityWebRequest uwr)
    {
        if (uwr.result == UnityWebRequest.Result.Success 
            || uwr.result == UnityWebRequest.Result.ProtocolError) { //Not sure why it still gets this kind of error
            Debug.Log("[onGetRequestCB] responseCode: " + uwr.responseCode);
            Debug.Log("[onGetRequestCB] downloadHandler.text: " + uwr.downloadHandler.text);
            switch (uwr.responseCode) {
                case 200:
                    Debug.Log("[onGetRequestCB] leaderboard fetched successfully! " + Id);
                    data = JsonUtility.FromJson<LeaderboardData>(uwr.downloadHandler.text);
                    LeaderboardsMgr.Inst.OnTabDataFetchingFinished(this);
                    break;
                case 400:
                    Debug.LogError("[onGetRequestCB] Json data not valid!!! " + uwr.responseCode);
                    break;
                case 401:
                    Debug.LogError("[onGetRequestCB] Token expired!!! " + uwr.responseCode);
                    break;
                default:
                    Debug.LogError("[onGetRequestCB] Unhandled response code!!! " + uwr.responseCode);
                    break;
            }
        } else {
            Debug.LogError("[onGetRequestCB] Unhandled response code!!! " + uwr.result);
        }
    }
}
