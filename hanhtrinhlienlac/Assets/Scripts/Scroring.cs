using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using WebGLSupport;

public class Scroring : SchoolApiRequestBase
{
    private static Scroring _inst;
    public static Scroring Inst {get => _inst;}
    private static readonly string SERVICE_SCORE_SUBMIT_PATH = "/api/v1/users/logScore";

    [Serializable] class ScoreSubmitData: PostData {
        public string gameId;
        public int bonusScore;
        public float finalScore;
    }

    [SerializeField] private TMPro.TextMeshProUGUI scoreTMPro;
    [SerializeField] private float maxRemainingTimeScore = 120f;
    [SerializeField] private float scoreLostPerSec = 1f;
    [SerializeField] private bool displayTotalScore = true;
    [SerializeField] private GamePausePopup pausePopup;


    private float curRemainingTimeScore;
    private float bonusScore;
    private bool isCounting = false;
    private float lastDisplayingScore;

    private bool pauseWhileCounting = false;

    public delegate bool IsCountableCheck();
    private IsCountableCheck isCountable = null;

    public int CurRemainingTimeScore {
        get => Mathf.RoundToInt(curRemainingTimeScore);
    }

    public int TotalScore {
        get => Mathf.RoundToInt(curRemainingTimeScore + bonusScore);
    }


    void Awake()
    {
        _inst = this;
        WebGLWindow.OnBlurEvent += OnWindowBlur;
        pausePopup?.RegisterOnResumeCB(OnUserResumeGame);
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCounting && CheckCountable() && curRemainingTimeScore > 0)
        {
            curRemainingTimeScore -= scoreLostPerSec * Time.deltaTime;
            if (curRemainingTimeScore < 0) 
            {
                curRemainingTimeScore = 0;
                UserInfo.GetInstance().SaveCurrentScene( SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("TimeUp");
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
        WebGLWindow.OnBlurEvent -= OnWindowBlur;
    }

    public void AddCountableCB(IsCountableCheck func)
    {
        isCountable += func;
    }

    private bool CheckCountable()
    {
        if (isCountable == null) {
            return true;
        }
        foreach (IsCountableCheck f in isCountable.GetInvocationList()) {
            if (!f()) {
                return false;
            }
        }
        return true;
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
        SendPostRequest(SERVICE_SCORE_SUBMIT_PATH, data, SchoolApiSession.Inst.AccessToken);
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

    protected override void onPostRequestCB(UnityWebRequest uwr)
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

    private void OnWindowBlur()
    {
        // if (pausePopup) {
        //     pausePopup.Show();
        //     if (isCounting) {
        //         pauseWhileCounting = true;
        //         Pause();
        //     }
        // }
    }
    
    private void OnUserResumeGame()
    {
        // if (pauseWhileCounting) {
        //     pauseWhileCounting = false;
        //     StartOrResume();
        // }
    }

}
