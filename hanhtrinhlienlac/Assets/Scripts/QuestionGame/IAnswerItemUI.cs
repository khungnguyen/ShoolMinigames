using UnityEngine;

interface IAnswerItemUI {
    public void SetInteractable(bool v);
    public void SetData(int index, ScriptableQuestion.Answer answer, System.Action<int> onSelectedCB);
    public void ResetSelection();
    public void HighlightCorrect(bool userCorrect);
    public void HighlightWrong();
}