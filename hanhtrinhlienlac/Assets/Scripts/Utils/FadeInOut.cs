using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float defaultDuration = 0.5f;
    private float targetAlpha = 1f;
    private float duration;
    private Action onFadeInFinishedCB;
    private Action onFadeOutFinishedCB;
    // Start is called before the first frame update
    void Start()
    {
        duration = defaultDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasGroup.alpha < targetAlpha) {
            canvasGroup.alpha += duration > 0 ? Time.deltaTime / duration : 1;
            if (canvasGroup.alpha >= targetAlpha) {
                canvasGroup.alpha = targetAlpha;
                onFadeInFinishedCB?.Invoke();
            }
        } else if (canvasGroup.alpha > targetAlpha) {
            canvasGroup.alpha -= duration > 0 ? Time.deltaTime / duration : 1;
            if (canvasGroup.alpha <= targetAlpha) {
                canvasGroup.alpha = targetAlpha;
                onFadeOutFinishedCB?.Invoke();
            }
        }
    }

    public void FadeIn(Action onFinishedCB = null, float overrideDuration = -1)
    {
        targetAlpha = 1f;
        duration = overrideDuration >= 0 ? overrideDuration : defaultDuration;
        onFadeInFinishedCB = onFinishedCB;
    }

    public void FadeOut(Action onFinishedCB = null, float overrideDuration = -1)
    {
        targetAlpha = 0f;
        duration = overrideDuration >= 0 ? overrideDuration : defaultDuration;
        if (duration == 0) {
            canvasGroup.alpha = targetAlpha;
            onFinishedCB?.Invoke();
        }
        onFadeOutFinishedCB = onFinishedCB;
    }
}
