using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MemoryGameItem : MonoBehaviour, IPointerClickHandler
{
    private static bool s_interactable = true;
    [SerializeField] private MemoryGameCard card;

    private int index;
    private Action<int> onClickedCB;
    private bool matched;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetData(int idx, Sprite sprite, Action<int> onItemClickedCB)
    {
        index = idx;
        card.SetFrontImage(sprite);
        onClickedCB = onItemClickedCB;
        matched = false;
    }

    public Sprite GetSprite()
    {
        return card.GetFrontImage();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!s_interactable || matched) return;
        onClickedCB(index);
    }

    public void SetState(bool open, float delaySec = 0f)
    {
        if (delaySec > 0) {
            s_interactable = false;
            StartCoroutine(SetStateWithDelay(open, delaySec));
        } else {
            card.SetState(open);
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
