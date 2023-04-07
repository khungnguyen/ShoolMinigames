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

    private string _curSelectedChar = "";
    private void Start()
    {
        _charInfoList.ForEach(e =>
        {
            e.uiItem.SetInfo(e).SetClickListener(OnCharSelect);
        });
        OnCharSelect(UserInfo.GetInstance().GetSkin());
    }

    public void OnCharSelect(string id)
    {
        _spine.Skeleton.SetSkin(id);
        _spine.Skeleton.SetSlotsToSetupPose();
        _spine.LateUpdate();
        _spine.AnimationState.SetAnimation(0, "victory", false);
        _spine.AnimationState.AddAnimation(0, "idle", true, 0);
        UserInfo.GetInstance().SetSkin(id);
        CharInfo info = _charInfoList.Find(e => e.charId.Equals(id));
        _textDetailed.SetText(info.info);
        _charInfoList.ForEach(e =>
        {
            e.uiItem.OnSelected(e.charId.Equals(id));
        });
        _curSelectedChar = id;
    }
    public void OnHide()
    {
        gameObject.SetActive(false);
    }
    public void OnShow()
    {
        gameObject.SetActive(true);
    }
    public void OnHelp() {
        CharInfo info = _charInfoList.Find(e => e.charId.Equals(_curSelectedChar));
        Application.OpenURL(info.url);
    }
}
[System.Serializable]
public struct CharInfo
{
    public string charId;
    public string charName;
    public string info;
    public Sprite avatar;
    public CharItem uiItem;
    public string url;
}
