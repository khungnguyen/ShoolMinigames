using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] TransitionEffect _transitionEffect;
    [SerializeField] TMP_Text _score;
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
    public void PlayTransitionEffect(Action a)
    {
        _transitionEffect.PlayEffect(a);
        // transitionBackground["MinGameSceneTransition"].time;
    }
    public void SetScore(int s)
    {
        _score.text = s.ToString();
    }
}
