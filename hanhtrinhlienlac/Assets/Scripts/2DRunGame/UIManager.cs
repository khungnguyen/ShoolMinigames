using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] TransitionEffect _transitionEffect;
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
}
