using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MemoryGameItem : MonoBehaviour, IPointerClickHandler
{
    private static bool s_interactable = true;
    [SerializeField] private MemoryGameCard cardUI;

    private ScriptableCard cardData;
    public ScriptableCard CardData { get => cardData; }
    private Action<MemoryGameItem> onClickedCB;
    private bool matched;
    public bool Matched { get => matched; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetData(ScriptableCard data, Action<MemoryGameItem> onItemClickedCB)
    {
        cardData = data;
        cardUI.SetFrontImage(data.sprite);
        onClickedCB = onItemClickedCB;
        matched = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!s_interactable || matched) return;
        onClickedCB(this);
    }

    public void SetState(bool open, float delaySec = 0f)
    {
        if (delaySec > 0) {
            s_interactable = false;
            StartCoroutine(SetStateWithDelay(open, delaySec));
        } else {
            cardUI.SetState(open);
        }
    }

    public void OnMatched()
    {
        matched = true;
    }

    public void ShowHide(bool show) {
        //Todo
    }

    private IEnumerator SetStateWithDelay(bool open, float delaySec)
    {
        yield return new WaitForSeconds(delaySec);
        SetState(open);
        s_interactable = true;
    }
}
