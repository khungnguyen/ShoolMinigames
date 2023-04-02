using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class TutorManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TutoriaType tutoriaType = TutoriaType.MainMenu;
    [SerializeField] float _speechSpeed = 0.1f;
    [SerializeField] TMP_Text _text;
    [SerializeField] TutorData _data;
    [SerializeField] bool _useDash = false;
    [SerializeField] bool _startAtBegin = true;
    [SerializeField] bool _useHelp = true;

    [SerializeField] AudioSource _audioSource;


    [SerializeField] GameObject _parentPanel;
    [SerializeField] GameObject _buttonShowHelp;
    private int _curTutStep = 0;

    private bool _isWriting = false;

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
        _curPart = _data.getAllParts().Find(e => e.type == tutoriaType);
        StartTutorial();
    }
    public TutorManager SetTutType(TutoriaType type)
    {
        tutoriaType = type;
        _curPart = _data.getAllParts().Find(e => e.type == tutoriaType);
        return this;
    }
    public void StartTutorial()
    {
        OnTutStart?.Invoke(tutoriaType);
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
        if (_isWriting)
        {
            StopAllCoroutines();
            UpdateText(_curPart.tuts[_curTutStep].text);
            _isWriting = false;
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
        OnTutComplete?.Invoke(tutoriaType);
    }
    private void UpdateText(string s)
    {
        _text.SetText(s);
    }
    IEnumerator DrawText(string s)
    {
        _isWriting = true;
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
        _isWriting = false;
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
}
public enum TutoriaType
{
    MainMenu,
    Quiz,
    Pair,
    Run2D_Game_1,
    Run2D_Game_2,
    Run2D_Game_3,
    Run2D_Game_4,
    Run2D_Game_End,
}