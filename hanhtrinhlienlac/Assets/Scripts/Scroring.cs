using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Scroring : SchoolApiRequestBase
{
    private static Scroring _inst;
    public static Scroring Inst {get => _inst;}
    private static readonly string ACCOUNT_SERVICE_SCORE_SUBMIT_PATH = "/api/v1/users/logScore";

    [Serializable] class ScoreSubmitData: RequestData {
        public string gameId;
        public int bonusScore;
        public float finalScore;
    }

    [SerializeField] private TMPro.TextMeshProUGUI scoreTMPro;
    [SerializeField] private float maxRemainingTimeScore = 120f;
    [SerializeField] private float scoreLostPerSec = 1f;
    [SerializeField] private bool displayTotalScore = true;


    private float curRemainingTimeScore;
    private float bonusScore;
    private bool isCounting = false;
    private float lastDisplayingScore;

    public int CurRemainingTimeScore {
        get => Mathf.RoundToInt(curRemainingTimeScore);
    }

    public int TotalScore {
        get => Mathf.RoundToInt(curRemainingTimeScore + bonusScore);
    }


    void Awake()
    {
        _inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCounting && curRemainingTimeScore > 0)
        {
            curRemainingTimeScore -= scoreLostPerSec * Time.deltaTime;
            if (curRemainingTimeScore < 0) 
            {
                curRemainingTimeScore = 0;
            }
        }

        int curDisplayingScore = displayTotalScore ? TotalScore : CurRemainingTimeScore;
        if (lastDisplayingScore != curDisplayingScore) {
            var diff = curDisplayingScore - lastDisplayingScore;
            var delta = diff < 2 ? diff : diff * Time.deltaTime / 0.2f;
            lastDisplayingScore += delta;
            UpdateVisual();
        }
    }

    void OnDestroy()
    {
        _inst = null;
    }

    public void StartOrResume()
    {
        isCounting = true;
    }

    public void Pause()
    {
        isCounting = false;
    }

    public void Reset()
    {
        isCounting = false;
        curRemainingTimeScore = maxRemainingTimeScore;
        bonusScore = 0f;
        UpdateVisual(true);
    }

    public void Submit(string gameId)
    {
        var data = new ScoreSubmitData() {
            gameId = gameId,
            bonusScore = 1,
            finalScore = TotalScore
        };

        Debug.LogWarning("Submiting score for gameId " + gameId + ". finalScore " + data.finalScore);
        PostRequest(ACCOUNT_SERVICE_SCORE_SUBMIT_PATH, data, SchoolApiSession.Inst.AccessToken);
    }

    public void AddBonusScore(float value)
    {
        bonusScore += value;
    }
    public void AddRemainingTimeScore(float value)
    {
        curRemainingTimeScore += value;
    }
    
    private void UpdateVisual(bool force = false)
    {
        if (force) {
            lastDisplayingScore = displayTotalScore ? TotalScore : CurRemainingTimeScore;
        }
        scoreTMPro.text = Mathf.RoundToInt(lastDisplayingScore).ToString();
    }

    protected override void onRequestCB(UnityWebRequest uwr)
    {
        if (uwr.result == UnityWebRequest.Result.Success 
            || uwr.result == UnityWebRequest.Result.ProtocolError) { //Not sure why it still gets this kind of error
            Debug.Log("[onRequestCB] responseCode: " + uwr.responseCode);
            Debug.Log("[onRequestCB] downloadHandler.text: " + uwr.downloadHandler.text);
            switch (uwr.responseCode) {
                case 200:
                    Debug.Log("[onRequestCB] Score submitted successfully!");
                    break;
                case 400:
                    Debug.LogError("Json data not valid!!! " + uwr.responseCode);
                    break;
                case 401:
                    Debug.LogError("Token expired!!! " + uwr.responseCode);
                    break;
                default:
                    Debug.LogError("Unhandled response code!!! " + uwr.responseCode);
                    break;
            }
        } else {
            Debug.LogError("Unhandled response code!!! " + uwr.result);
        }
    }

    protected override void onGetRequestCB(UnityWebRequest uwr)
    {
        throw new NotImplementedException();
    }
}
