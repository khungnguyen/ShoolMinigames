using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class AnswerItemUI : MonoBehaviour, IPointerClickHandler, IAnswerItemUI
{
    private static string[] ids = new[] {"A", "B", "C", "D", "E", "G", "H", "I", "K"};
    [SerializeField] private TextMeshProUGUI idTMPro;
    [SerializeField] private TextMeshProUGUI textTMPro;
    [SerializeField] private Image bgImage;
    [SerializeField] private Sprite bgSpriteSelected;
    [SerializeField] private GameObject correctMark;
    [SerializeField] private GameObject incorrectMark;
    [SerializeField] private ScoreEarningAnim scoreEarningAnim;

    private Sprite bgSpriteDefault;
    private Color textColorDefault;

    private int index;
    private Action<int> onSelectedCB;
    private bool interactable;

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
        ResetSelection();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactable)
        {
            OnSelected();
        }
    }

    public void ResetSelection()
    {
        bgImage.sprite = bgSpriteDefault;
        bgImage.color = Color.white;
        textTMPro.color = textColorDefault;
        correctMark.SetActive(false);
        incorrectMark.SetActive(false);
    }

    public void HighlightCorrect(bool userCorrect)
    {
        bgImage.sprite = bgSpriteSelected;
        bgImage.color = Color.green;
        correctMark.SetActive(userCorrect);
        if (userCorrect) {
            scoreEarningAnim.Play();
        }
    }

    public void HighlightWrong()
    {
        bgImage.sprite = bgSpriteSelected;
        bgImage.color = Color.red;
        incorrectMark.SetActive(true);
    }

    public void SetInteractable(bool v)
    {
        interactable = v;
    }

    private void OnSelected()
    {
        bgImage.sprite = bgSpriteSelected;
        textTMPro.color = Color.yellow;
        onSelectedCB(index);
    }
}
