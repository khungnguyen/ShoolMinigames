using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using TMPro;
using UnityEngine.UI;

public class CharSelectorUI : MonoBehaviour
{
    [SerializeField] SkeletonGraphic _spine;
    [SerializeField] List<CharInfo> _charInfoList;
    [SerializeField] TMP_Text _textDetailed;
    private void Start() {
        OnCharSelect("char_1");
        _charInfoList.ForEach(e=>{
            e.uiItem.SetInfo(e).SetClickListener(OnCharSelect);
        });
    }
    public void OnCharSelect(string id)
    {
        _spine.Skeleton.SetSkin(id);
        _spine.Skeleton.SetSlotsToSetupPose();
        _spine.LateUpdate();
         _spine.AnimationState.SetAnimation(0, "victory", false);
         _spine.AnimationState.AddAnimation(0, "idle", true,0);
         UserInfo.GetInstance().SetSkin(id);
        CharInfo info = _charInfoList.Find(e=>e.charId.Equals(id));
        _textDetailed.SetText(info.info);
    }
    public void OnHide() {
        gameObject.SetActive(false);
    }
    public void OnShow() {
        gameObject.SetActive(true);
    }
}
[System.Serializable]
public struct CharInfo {
    public string charId;
    public string charName;
    public string info;
    public Sprite avatar;
    public Sprite avatarSelected;
    public CharItem uiItem;
}
