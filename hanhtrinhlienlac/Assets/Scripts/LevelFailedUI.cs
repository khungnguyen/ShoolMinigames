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
        StartCoroutine(PlaySfxIEnumerator(_soundManager.soundData[1], false, -1));
        StartCoroutine(LoadSceneIEnumerator(targetScene, 0.2f));

    }
    void Start()
    {
        StartCoroutine(PlaySfxIEnumerator(_soundManager.soundData[0], false, 0));
    }

    void OnEnable()
    {
        _animation?.PlayBoundInEffect();
    }

    IEnumerator PlaySfxIEnumerator(AudioClip ac, bool loop, int channel) 
    {
        yield return new WaitForEndOfFrame();
        _soundManager.PlaySfx(ac, loop, channel);
    }

    IEnumerator LoadSceneIEnumerator(string scenename, float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scenename);
    }
}
