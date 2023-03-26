using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestionGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionTMPro;
    [SerializeField] private Transform answersContainer;
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private Button submitBtn;
    [SerializeField] private QuestionGameResultPopup resultPopup;
    private Color submitBtnColorEnabled;

    private int curQuestionIndex = -1;
    private Question curQuestion;
    private int selectedAnswerIndex = -1;

    private int finishedCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        submitBtnColorEnabled = submitBtn.GetComponent<Image>().color;
        ShowNextQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonBackToMapClicked()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnButtonNextClicked()
    {
        if (IsFinished()) {
            OnButtonBackToMapClicked();
            return;
        }
        ShowNextQuestion();
    }

    public void ShowNextQuestion() {
        HideResultPopup();
        SetSubmitButtonEnable(false);

        curQuestionIndex++;
        Debug.Assert(curQuestionIndex < QuestionBank.Inst.questions.Length);

        curQuestion = QuestionBank.Inst.questions[curQuestionIndex];

        questionTMPro.text = curQuestion.text;
        
        int idx = 0;
        // Set answers' data
        for (; idx < curQuestion.answers.Length; idx++) {
            var answerItemUI = GetAnswerItemUI(idx);
            answerItemUI.gameObject.SetActive(true);
            answerItemUI.SetData(idx, curQuestion.answers[idx].text, OnAnswerSelected);
            answerItemUI.ResetSelection();
        }
        // Deactivate unused answer slots
        for (; idx < answersContainer.childCount; idx++) {
            answersContainer.GetChild(idx).gameObject.SetActive(false);
        }
    }

    public void OnAnswerSelected(int answerIndex)
    {
        selectedAnswerIndex = answerIndex;
        for (int i = 0; i < curQuestion.answers.Length; i++) {
            if (i != selectedAnswerIndex) {
               GetAnswerItemUI(i).ResetSelection();
            }
        }
        SetSubmitButtonEnable(true);
    }

    public void OnSubmitBtnClicked()
    {
        finishedCount++;
        bool result = curQuestion.answers[selectedAnswerIndex].value;
        Debug.Log("Your answer is " + result);

        if (result) {
            GetAnswerItemUI(selectedAnswerIndex).HighlightCorrect();
        } else {
            int correctIdx = GetCorrectAnswerIndex(curQuestion);
            GetAnswerItemUI(correctIdx).HighlightCorrect();
            GetAnswerItemUI(selectedAnswerIndex).HighlightWrong();
        }
        StartCoroutine(ShowResultPopup(result, 0.5f));
    }

    private void SetSubmitButtonEnable(bool enable)
    {
        submitBtn.enabled = enable;
        var bgImage = submitBtn.GetComponent<Image>();
        bgImage.color = enable ? submitBtnColorEnabled : Color.gray;
    }

    private IEnumerator ShowResultPopup(bool result, float delaySec = 0f)
    {
        if (delaySec > 0) {
            yield return new WaitForSeconds(delaySec);
        }
        resultPopup.Show(result, IsFinished());
    }

    private void HideResultPopup() {
        resultPopup.Hide();
    }

    private bool IsFinished()
    {
        return finishedCount >= QuestionBank.Inst.questions.Length;
    }

    private AnswerItemUI GetAnswerItemUI(int idx)
    {
        return answersContainer.GetChild(idx).GetComponent<AnswerItemUI>();
    }

    private int GetCorrectAnswerIndex(Question question)
    {
        for (int i = 0; i < question.answers.Length; i++) {
            if (question.answers[i].value) {
                return i;
            }
        }
        return -1;
    }
}
