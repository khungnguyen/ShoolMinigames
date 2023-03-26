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
    [SerializeField] private Color bgColorSelected;
    private Color bgColorDefault;
    private Color textColorDefault;

    private int index;
    private Action<int> onSelectedCB;

    void Awake() {
        bgColorDefault = bgImage.color;
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
        bgImage.color = bgColorDefault;
        textTMPro.color = textColorDefault;
    }

    public void HighlightCorrect()
    {
        bgImage.color = Color.green;
    }

    public void HighlightWrong()
    {
        bgImage.color = Color.red;
    }

    private void OnSelected()
    {
        bgImage.color = bgColorSelected;
        textTMPro.color = bgColorDefault;
        onSelectedCB(index);
    }
}
