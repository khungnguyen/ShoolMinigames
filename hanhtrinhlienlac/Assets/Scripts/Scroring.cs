using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroring : MonoBehaviour
{
    private static Scroring _inst;
    public static Scroring Inst {get => _inst;}
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
}
