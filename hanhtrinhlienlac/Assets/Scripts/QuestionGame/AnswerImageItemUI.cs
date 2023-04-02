using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerImageItemUI : MonoBehaviour, IPointerClickHandler, IAnswerItemUI
{
    [SerializeField] private Image bgImage;
    [SerializeField] private Color colorSelected;
    [SerializeField] private Color colorCorrect;
    [SerializeField] private Color colorWrong;
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
    }

    public void SetData(int index, string text, Action<int> onSelectedCB) {
        this.index = index;
        this.onSelectedCB = onSelectedCB;
    }

    public void ResetSelection()
    {
        bgImage.color = Color.white;
    }

    public void HighlightCorrect()
    {
        bgImage.color = colorCorrect;
    }

    public void HighlightWrong()
    {
        bgImage.color = colorWrong;
    }

    private void OnSelected()
    {
        bgImage.color = colorSelected;
        onSelectedCB(index);
    }
}
