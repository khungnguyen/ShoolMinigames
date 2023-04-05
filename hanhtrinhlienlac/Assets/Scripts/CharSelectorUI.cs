using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class CharSelectorUI : MonoBehaviour
{
    [SerializeField] SkeletonGraphic  _spine;
    public void OnCharSelect(string id) {
        _spine.Skeleton.SetSkin(id);
    }
}
