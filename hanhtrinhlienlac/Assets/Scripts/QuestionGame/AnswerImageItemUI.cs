using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerImageItemUI : MousePointerHandler, IPointerClickHandler, IAnswerItemUI
{
    [SerializeField] private Image bgImage;
    [SerializeField] private Image bgFrame;
    [SerializeField] private Image image;
    [SerializeField] private Color colorSelected;
    [SerializeField] private Color colorCorrect;
    [SerializeField] private Color colorWrong;
    [SerializeField] private GameObject correctMark;
    [SerializeField] private GameObject incorrectMark;
    [SerializeField] private ScoreEarningAnim scoreEarningAnim;

    private int index;
    private Action<int> onSelectedCB;
    private bool interactable;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactable)
        {
            OnSelected();
        }
    }

    public void SetInteractable(bool v)
    {
        interactable = v;
        CursorChangable = v;
    }

    public void SetData(int index, ScriptableQuestion.Answer answer, Action<int> onSelectedCB) {
        this.index = index;
        image.sprite = answer.img;
        image.preserveAspect = true;
        this.onSelectedCB = onSelectedCB;
        ResetSelection();
    }

    public void ResetSelection()
    {
        bgFrame.color = Color.white;
        correctMark.SetActive(false);
        incorrectMark.SetActive(false);
    }

    public void HighlightCorrect(bool userCorrect)
    {
        bgFrame.color = colorCorrect;
        correctMark.SetActive(true);
        if (userCorrect) {
            scoreEarningAnim.Play();
        }
    }

    public void HighlightWrong()
    {
        // bgFrame.color = colorWrong;
        incorrectMark.SetActive(true);
    }

    private void OnSelected()
    {
        bgFrame.color = colorSelected;
        onSelectedCB(index);
    }
}
