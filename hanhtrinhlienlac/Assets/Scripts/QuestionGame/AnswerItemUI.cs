using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class AnswerItemUI : MonoBehaviour, IPointerClickHandler
{
    private static string[] ids = new[] {"A", "B", "C", "D", "E", "G", "H", "I", "K"};
    [SerializeField] private TextMeshProUGUI idTMPro;
    [SerializeField] private TextMeshProUGUI textTMPro;
    [SerializeField] private Image bgImage;
    [SerializeField] private Sprite bgSpriteSelected;
    private Sprite bgSpriteDefault;
    private Color textColorDefault;

    private int index;
    private Action<int> onSelectedCB;

    void Awake() {
        bgSpriteDefault = bgImage.sprite;
        textColorDefault = textTMPro.color;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetData(int index, string text, Action<int> onSelectedCB) {
        this.index = index;
        idTMPro.text = ids[index];
        textTMPro.text = text;
        this.onSelectedCB = onSelectedCB;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelected();
    }

    public void ResetSelection()
    {
        bgImage.sprite = bgSpriteDefault;
        bgImage.color = Color.white;
        textTMPro.color = textColorDefault;
    }

    public void HighlightCorrect()
    {
        bgImage.sprite = bgSpriteSelected;
        bgImage.color = Color.green;
    }

    public void HighlightWrong()
    {
        bgImage.sprite = bgSpriteSelected;
        bgImage.color = Color.red;
    }

    private void OnSelected()
    {
        bgImage.sprite = bgSpriteSelected;
        textTMPro.color = Color.yellow;
        onSelectedCB(index);
    }
}
