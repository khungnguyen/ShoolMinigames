using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] TransitionEffect _transitionEffect;
    [SerializeField] TutorManager _tutor;
    [SerializeField] Scroring _score;
    [SerializeField] Transform _coinHubTarget;
    [SerializeField] RewardUI _rewardUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GoBackToMainMenu()
    {
        if (Defined.CHEAT_BACK_AS_FINISHED)
        {
            UserInfo.GetInstance().SetCompletedRunGame(true);
            UserInfo.GetInstance().SetUnlockLevel(CheckPointType.CHECK_POINT_4, true);
        }
        SceneManager.LoadScene("Main");
    }
    public void PlayTransitionEffect(bool isEnter, Action a)
    {
        _transitionEffect.PlayEffect(isEnter, a);
        // transitionBackground["MinGameSceneTransition"].time;
    }
    public void SetScore(int s)
    {
        _score.AddRemainingTimeScore(s);
    }
    public TutorManager getTutorManager()
    {
        return _tutor;
    }
    public void StartScoring()
    {
        _score.StartOrResume();
    }
    public void PauseScoring()
    {
        _score.Pause();
    }
    public Vector2 GetCoinHubWorldPos()
    {
        return _coinHubTarget.position;
    }
    public Scroring GetScoring()
    {
        return _score;
    }
    public RewardUI GetRewardUI()
    {
        return _rewardUI;
    }
}
