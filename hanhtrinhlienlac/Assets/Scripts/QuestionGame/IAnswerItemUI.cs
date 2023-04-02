using UnityEngine;

interface IAnswerItemUI {
    public void SetInteractable(bool v);
    public void SetData(int index, string text, System.Action<int> onSelectedCB);
    public void ResetSelection();
    public void HighlightCorrect();
    public void HighlightWrong();
}