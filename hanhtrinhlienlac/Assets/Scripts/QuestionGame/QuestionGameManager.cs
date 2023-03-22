using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionTMPro;
    [SerializeField] private Transform answersContainer;
    [SerializeField] private GameObject answerPrefab;

    private Question curQuestion;

    // Start is called before the first frame update
    void Start()
    {
        ShowNextQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNextQuestion() {
        curQuestion = QuestionBank.Inst.questions[0];

        questionTMPro.text = curQuestion.text;
        
        int idx = 0;
        // Set answers' data
        for (; idx < curQuestion.answers.Length; idx++) {
            var answerItemUI = answersContainer.GetChild(idx).GetComponent<AnswerItemUI>();
            answerItemUI.gameObject.SetActive(true);
            answerItemUI.SetData(idx, curQuestion.answers[idx].text);
        }
        // Deactivate unused answer slots
        for (; idx < answersContainer.childCount; idx++) {
            answersContainer.GetChild(idx).gameObject.SetActive(false);
        }
    }
}
