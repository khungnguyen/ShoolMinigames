using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;

public class MemoryGameResultPopup : MonoBehaviour
{
    [SerializeField] private GameObject msgVuotQuaThuThach;
    [SerializeField] private GameObject msgMatThuFound;
    [SerializeField] private GameObject msgFishingRodFound;
    [SerializeField] private Image extraInfoImage;
    [SerializeField] private GameObject btnNext;
    [SerializeField] private SkeletonGraphic spine;
    private string secretCode ="mat thu";
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Show(bool finished, Sprite extraImage = null, bool isFishingRod = false)
    {
        gameObject.SetActive(true);
        if (finished)
        {
            btnNext.SetActive(false);
        }

        //extra info
        if (extraImage)
        {
            extraInfoImage.gameObject.SetActive(true);
            extraInfoImage.sprite = extraImage;

            msgVuotQuaThuThach.SetActive(false);
            msgFishingRodFound.SetActive(isFishingRod);
            msgMatThuFound.SetActive(!isFishingRod);
            spine.gameObject.SetActive(false);
        }
        else
        {
            extraInfoImage.enabled = false;
            spine.gameObject.SetActive(true);
            ChangeSkin("char_1");
            SetAnimation(secretCode);
            msgVuotQuaThuThach.SetActive(true);
            msgFishingRodFound.SetActive(false);
            msgMatThuFound.SetActive(false);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
     public void ChangeSkin(string s)
    {
        spine.Skeleton.SetSkin(s);
        spine.Skeleton.SetSlotsToSetupPose();
        spine.LateUpdate();
    }
    public void SetAnimation(string anim, bool loop = false, Action completed = null)
    {
        var track = spine.AnimationState.SetAnimation(0, anim, loop);
        track.Complete += (t) =>
        {
            completed?.Invoke();
        };
    }
}
