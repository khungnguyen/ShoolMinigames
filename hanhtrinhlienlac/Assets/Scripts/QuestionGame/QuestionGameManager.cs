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
    [SerializeField] private Transform imageAnswersContainer;
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private QuestionGameResultPopup resultPopup;
    [SerializeField] private float resultShowingDuration = 2;
    [SerializeField] private TutorManager tutorComp;

    private int curQuestionIndex = -1;
    private Question curQuestion;
    private int selectedAnswerIndex = -1;

    private int finishedCount = 0;

    private Transform curAnswerContainer;
    
    void Start()
    {
        tutorComp.OnTutComplete += (t) =>
        {
            ShowNextQuestion();
        };
        tutorComp.OnTutStart += (t) =>
        {
            Scroring.Inst.Pause();
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonBackToMapClicked()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnMoreInfoButtonClicked()
    {
        Application.OpenURL(curQuestion.moreInfoURL);
    }

    public void OnButtonNextClicked()
    {
        if (IsFinished())
        {
            OnButtonBackToMapClicked();
            return;
        }
        ShowNextQuestion();
    }

    public void ShowNextQuestion()
    {
        Scroring.Inst.StartOrResume();

        HideResultPopup();

        curQuestionIndex++;
        Debug.Assert(curQuestionIndex < QuestionBank.Inst.questions.Length);

        curQuestion = QuestionBank.Inst.questions[curQuestionIndex];

        if (curQuestion.anwserByImage)
        {
            curAnswerContainer = imageAnswersContainer;
            answersContainer.gameObject.SetActive(false);
        }
        else
        {
            curAnswerContainer = answersContainer;
            imageAnswersContainer.gameObject.SetActive(false);
        }
        curAnswerContainer.gameObject.SetActive(true);

        questionTMPro.text = curQuestion.text;

        int idx = 0;
        // Set answers' data
        for (; idx < curQuestion.answers.Length; idx++)
        {
            curAnswerContainer.GetChild(idx).gameObject.SetActive(true);
            var answerItemUI = GetAnswerItemUI(idx);
            answerItemUI.SetData(idx, curQuestion.answers[idx].text, OnAnswerSelected);
            answerItemUI.ResetSelection();
            answerItemUI.SetInteractable(true);
        }
        // Deactivate unused answer slots
        for (; idx < curAnswerContainer.childCount; idx++)
        {
            curAnswerContainer.GetChild(idx).gameObject.SetActive(false);
        }
    }

    public void OnAnswerSelected(int answerIndex)
    {
        selectedAnswerIndex = answerIndex;
        for (int i = 0; i < curQuestion.answers.Length; i++)
        {
            var itemUI = GetAnswerItemUI(i);
            if (i != selectedAnswerIndex)
            {
                itemUI.ResetSelection();
            }
            itemUI.SetInteractable(false);
        }

        Scroring.Inst.Pause();
        // show result then move to next question
        StartCoroutine(SubmitWithDelay(1));
    }

    private IEnumerator SubmitWithDelay(float delaySec)
    {
        yield return new WaitForSeconds(delaySec);

        finishedCount++;
        bool result = curQuestion.answers[selectedAnswerIndex].value;
        Debug.Log("Your answer is " + result);

        if (result)
        {
            GetAnswerItemUI(selectedAnswerIndex).HighlightCorrect(true);
            Scroring.Inst.AddRemainingTimeScore(curQuestion.score);
        }
        else
        {
            int correctIdx = GetCorrectAnswerIndex(curQuestion);
            GetAnswerItemUI(correctIdx).HighlightCorrect(false);
            GetAnswerItemUI(selectedAnswerIndex).HighlightWrong();
        }

        yield return new WaitForSeconds(resultShowingDuration);

        if (IsFinished())
        {
            StartCoroutine(ShowResultPopup(result, 0));
        }
        else
        {
            ShowNextQuestion();
        }
    }

    private IEnumerator ShowResultPopup(bool result, float delaySec = 0f)
    {
        if (delaySec > 0)
        {
            yield return new WaitForSeconds(delaySec);
        }
        resultPopup.Show(result, IsFinished());
    }

    private void HideResultPopup()
    {
        resultPopup.Hide();
    }

    private bool IsFinished()
    {
        return finishedCount >= QuestionBank.Inst.questions.Length;
    }

    private IAnswerItemUI GetAnswerItemUI(int idx)
    {
        return curAnswerContainer.GetChild(idx).GetComponent<IAnswerItemUI>();
    }

    private int GetCorrectAnswerIndex(Question question)
    {
        for (int i = 0; i < question.answers.Length; i++)
        {
            if (question.answers[i].value)
            {
                return i;
            }
        }
        return -1;
    }
}
