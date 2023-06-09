using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class QuestionGameManager : MonoBehaviour
{
    [Serializable] struct AudioClips {
        public AudioClip bgm;
        public AudioClip nextQuestionSFX;
        public AudioClip clickSFX;
        public AudioClip scoringSFX;
        public AudioClip wrongSFX;
        public AudioClip levelFinishSFX;
    }
    [SerializeField] private TextMeshProUGUI questionTMPro;
    [SerializeField] private Transform answersContainer;
    [SerializeField] private Transform imageAnswersContainer;
    [SerializeField] private GameObject answerPrefab;
    // [SerializeField] private QuestionGameResultPopup resultPopup;
    [SerializeField] private RewardUI rewardUI;
    [SerializeField] private float resultShowingDuration = 2;
    [SerializeField] private TutorManager tutorComp;
    [SerializeField] private SoundManager soundMgr;
    [SerializeField] private AudioClips audioClips;
    [SerializeField] private List<ScriptableQuestion> questions;
    [SerializeField] private ImgSlideXHCN imgSlide;

    private int curQuestionIndex = -1;
    private ScriptableQuestion curQuestion;
    private int selectedAnswerIndex = -1;

    private int finishedCount = 0;
    private int failedCount = 0;

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

        StartCoroutine(PlayBGMDelay(audioClips.bgm, true));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonBackToMapClicked()
    {
        soundMgr.PlaySfx(audioClips.clickSFX, false, 1);
        SceneManager.LoadScene("Main");
        if (Defined.CHEAT_BACK_AS_FINISHED)
        {
            UserInfo.GetInstance().SetUnlockLevel(CheckPointType.CHECK_POINT_2, true);
        }
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
        soundMgr.PlaySfx(audioClips.nextQuestionSFX, false, 1);

        curQuestionIndex++;
        Debug.Assert(curQuestionIndex < questions.Count);

        curQuestion = questions[curQuestionIndex];
        curQuestion.Init();
        imgSlide.SetSprites(curQuestion.extraImages);

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
        for (; idx < curQuestion.answers.Count; idx++)
        {
            curAnswerContainer.GetChild(idx).gameObject.SetActive(true);
            var answerItemUI = GetAnswerItemUI(idx);
            answerItemUI.SetData(idx, curQuestion.answers[idx], OnAnswerSelected);
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
        for (int i = 0; i < curQuestion.answers.Count; i++)
        {
            var itemUI = GetAnswerItemUI(i);
            if (i != selectedAnswerIndex)
            {
                itemUI.ResetSelection();
            }
            itemUI.SetInteractable(false);
        }

        Scroring.Inst.Pause();
        soundMgr.PlaySfx(audioClips.clickSFX);
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
            soundMgr.PlaySfx(audioClips.scoringSFX);
        }
        else
        {
            failedCount++;
            int correctIdx = GetCorrectAnswerIndex(curQuestion);
            GetAnswerItemUI(correctIdx).HighlightCorrect(false);
            GetAnswerItemUI(selectedAnswerIndex).HighlightWrong();
            soundMgr.PlaySfx(audioClips.wrongSFX);
        }

        if (IsFinished()) {
            Scroring.Inst.Submit("game1");
        }

        float delay = failedCount >= 3 ? 1f : resultShowingDuration;

        yield return new WaitForSeconds(delay);

        if (failedCount >= 3) {
            SceneManager.LoadScene("Level1_Failed");
        } else {
            if (IsFinished())
            {
                StartCoroutine(ShowRewardPopup(result, 0));
            }
            else
            {
                ShowNextQuestion();
            }
        }
    }

    private IEnumerator PlayBGMDelay(AudioClip ac, bool loop)
    {
        yield return new WaitForEndOfFrame();
        soundMgr.PlayBGM(ac, loop);
    }

    private IEnumerator ShowRewardPopup(bool result, float delaySec = 0f)
    {
        if (delaySec > 0)
        {
            yield return new WaitForSeconds(delaySec);
        }
        soundMgr.PlaySfx(audioClips.levelFinishSFX, false, 1);
        rewardUI.Show(Scroring.Inst.TotalScore.ToString(), OnButtonBackToMapClicked);
        UserInfo.GetInstance().SetUnlockLevel(CheckPointType.CHECK_POINT_2,true);
    }

    private bool IsFinished()
    {
        return finishedCount >= questions.Count;
    }

    private IAnswerItemUI GetAnswerItemUI(int idx)
    {
        return curAnswerContainer.GetChild(idx).GetComponent<IAnswerItemUI>();
    }

    private int GetCorrectAnswerIndex(ScriptableQuestion question)
    {
        for (int i = 0; i < question.answers.Count; i++)
        {
            if (question.answers[i].value)
            {
                return i;
            }
        }
        return -1;
    }
}
