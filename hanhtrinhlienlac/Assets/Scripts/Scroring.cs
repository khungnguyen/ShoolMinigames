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

    public float CurRemainingTimeScore {
        get => Mathf.RoundToInt(curRemainingTimeScore);
    }

    public float TotalScore {
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
        UpdateVisual();
    }

    public void AddBonusScore(float value)
    {
        bonusScore += value;
    }
    public void AddRemainingTimeScore(float value)
    {
        curRemainingTimeScore += value;
    }
    private void UpdateVisual()
    {
        scoreTMPro.text = displayTotalScore ? TotalScore.ToString() : CurRemainingTimeScore.ToString();
    }
}
