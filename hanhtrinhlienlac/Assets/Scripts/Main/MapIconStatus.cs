using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class MapIconStatus : MonoBehaviour
{
    public GameObject r;

    private Color lockedColor = new Color(0.3018868f, 0.3018868f, 0.3018868f, 1f);
    private Color unlockedColor = new Color(1, 1, 1, 1);
    public void SetStatus(bool isUnlocked)
    {
        if (r.TryGetComponent<SpriteRenderer>(out SpriteRenderer spr))
        {
            spr.color = isUnlocked ? unlockedColor : lockedColor;
        }
        else if (r.TryGetComponent<SkeletonAnimation>(out SkeletonAnimation ske))
        {
            Debug.LogError(ske.skeleton.R);
            ske.skeleton.R = !isUnlocked?lockedColor.r:unlockedColor.r;
            ske.skeleton.G = !isUnlocked?lockedColor.g:unlockedColor.g;
            ske.skeleton.B = !isUnlocked?lockedColor.b:unlockedColor.b;
            ske.skeleton.A = !isUnlocked?lockedColor.a:unlockedColor.a;
        }

    }

}
