using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class TutorManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TutoriaType tutorialType = TutoriaType.MainMenu;
    [SerializeField] float _speechSpeed = 0.1f;
    [SerializeField] TMP_Text _text;
    [SerializeField] TMP_Text _textContinues;
    [SerializeField] TutorData _data;
    [SerializeField] bool _useDash = false;
    [SerializeField] bool _startAtBegin = true;
    [SerializeField] bool _useHelp = true;

    [SerializeField] AudioSource _audioSource;


    [SerializeField] GameObject _parentPanel;
    [SerializeField] GameObject _buttonShowHelp;
    [SerializeField] List<Transform> _speakers;
    private int _curTutStep = 0;
    private bool _isWriting = false;
    private bool isWriting
    {
        set
        {
            _isWriting = value;
            ShowContinuesText(!value);
        }
        get { return _isWriting; }
    }

    private bool _completed = false;

    public Action<TutoriaType> OnTutComplete;
    public Action<TutoriaType> OnTutStart;
    private TutPart _curPart;
    void Awake()
    {
        gameObject.SetActive(_startAtBegin);
    }
    void Start()
    {
        if (tutorialType != TutoriaType.NA)
        {
            SetTutType(tutorialType);
            StartTutorial();
        }
        ShowContinuesText(false);

    }
    public TutorManager SetTutType(TutoriaType type)
    {
        tutorialType = type;
        _curPart = _data.getAllParts().Find(e => e.type == type);
        _speakers.ForEach(e => e.gameObject.SetActive(e.name.Equals(_curPart.speaker.ToString())));
        //Debug.LogError("type" + type.ToString());
        return this;
    }
    public void StartTutorial()
    {
        StopAllCoroutines();
        OnTutStart?.Invoke(tutorialType);
        ShowButtonHelp(false);
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        reset();
        StartSpeechWordByWord();
    }
    private void StartSpeechWordByWord()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        if (_curPart.tuts[_curTutStep].audio != null)
        {
            _audioSource?.PlayOneShot(_curPart.tuts[_curTutStep].audio, 1);
        }
        StartCoroutine(DrawText(_curPart.tuts[_curTutStep].text));
    }

    public void onClick()
    {
        if (_completed) return;
        if (isWriting)
        {
            StopAllCoroutines();
            UpdateText(_curPart.tuts[_curTutStep].text);
            isWriting = false;
        }
        else
        {
            if (_curPart.tuts.Count - 1 > _curTutStep)
            {
                _curTutStep++;
                StartSpeechWordByWord();
            }
            else
            {
                EndTutorial();
            }

        }

    }
    private void EndTutorial()
    {
        _completed = true;
        ShowTutor(false);
        ShowButtonHelp(true);
        StopAllCoroutines();
        OnTutComplete?.Invoke(tutorialType);
    }
    private void UpdateText(string s)
    {
        _text.SetText(s);
    }
    IEnumerator DrawText(string s)
    {
        isWriting = true;
        int count = 0;
        string postfix = "";
        while (true)
        {
            if (count > s.Length) break;
            yield return new WaitForSeconds(_speechSpeed);
            string t = s[..count++];
            if (_useDash)
            {
                postfix = (s.Length == count - 1) ? "" : " _";
            }
            UpdateText(t + postfix);
        }
        isWriting = false;

    }
    public void ShowTutor(bool enable)
    {
        _parentPanel.SetActive(enable);
        if (enable)
        {
            StartTutorial();
        }
        else
        {
            UpdateText(""); // clear text
        }
    }
    public void ShowButtonHelp(bool e)
    {
        _buttonShowHelp.SetActive(_useHelp && e);
    }
    public void reset()
    {
        _curTutStep = 0;
        _completed = false;
    }
    private void ShowContinuesText(bool show)
    {
        var anim = _textContinues.GetComponent<Animator>();
        _textContinues.gameObject.SetActive(show);
        if (show)
        {
            anim.Play("BlinkText",-1,0f);
        }
        else
        {
         //   anim.;
        }
        


    }
}
public enum TutoriaType
{
    NA,
    MainMenu,
    Quiz,
    Pair,
    Run2D_Game_1,
    Run2D_Game_2,
    Run2D_Game_3,
    Run2D_Game_4,
    Run2D_Game_End,
}