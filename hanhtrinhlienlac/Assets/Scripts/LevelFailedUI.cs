using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelFailedUI : MonoBehaviour
{
    [SerializeField] BoundInAndOut _animation;
    [SerializeField] SoundManager _soundManager;
    [SerializeField] string targetScene;

    public void OnBtnRetryClicked()
    {
        _soundManager.PlaySfx(_soundManager.soundData[1]);
        UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);

    }
    void Start()
    {
        _soundManager.PlaySfx(_soundManager.soundData[0],false,-1,1,1);
    }

    void OnEnable()
    {
        _animation?.PlayBoundInEffect();
    }
}
