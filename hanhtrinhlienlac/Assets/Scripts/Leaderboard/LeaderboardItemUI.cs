using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardItemUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI rankTMP;
    [SerializeField] private TMPro.TextMeshProUGUI playerNameTMP;
    [SerializeField] private TMPro.TextMeshProUGUI scoreTMP;
    [SerializeField] private bool forLocalPlayer = false;

    public bool IsForLocalPlayer { get => forLocalPlayer; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInfo(LeaderboardItemInfo info)
    {
        rankTMP.text = info.rank.ToString();
        playerNameTMP.text = info.username.ToString();
        scoreTMP.text = info.DisplayingScore.ToString();
    }
}
