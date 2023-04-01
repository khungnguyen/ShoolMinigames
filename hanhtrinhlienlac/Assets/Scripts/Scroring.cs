using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroring : MonoBehaviour
{
    private static Scroring _inst;
    public static Scroring Inst {get => _inst;}
    [SerializeField] private TMPro.TextMeshProUGUI scoreTMPro;
    [SerializeField] private float maxScore = 120f;
    [SerializeField] private float scoreLostPerSec = 1f;


    private float curScore;
    private bool isCounting = false;

    public float CurScore {
        get => Mathf.RoundToInt(curScore);
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
        if (isCounting && curScore > 0)
        {
            curScore -= scoreLostPerSec * Time.deltaTime;
            if (curScore < 0) 
            {
                curScore = 0;
            }

            scoreTMPro.text = CurScore.ToString();
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
        curScore = maxScore;
        scoreTMPro.text = CurScore.ToString();
    }
}
