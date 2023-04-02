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
        SceneManager.LoadScene("Main");
    }
    public void PlayTransitionEffect(bool isEnter,Action a)
    {
        _transitionEffect.PlayEffect(isEnter,a);
        // transitionBackground["MinGameSceneTransition"].time;
    }
    public void SetScore(int s)
    {
       _score.AddRemainingTimeScore(s);
    }
    public TutorManager getTutorManager() {
        return _tutor;
    }
    public void StartScoring() {
        _score.StartOrResume();
    }
}
