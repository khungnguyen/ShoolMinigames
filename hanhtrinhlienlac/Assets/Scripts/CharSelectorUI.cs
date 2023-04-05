using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class CharSelectorUI : MonoBehaviour
{
    [SerializeField] SkeletonGraphic _spine;
    public void OnCharSelect(string id)
    {
        _spine.Skeleton.SetSkin(id);
        _spine.Skeleton.SetSlotsToSetupPose();
        _spine.LateUpdate();
         _spine.AnimationState.SetAnimation(0, "victory", false);
         _spine.AnimationState.AddAnimation(0, "idle", true,0);
    }
}
