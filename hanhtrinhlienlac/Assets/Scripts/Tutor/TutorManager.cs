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


    [SerializeField] GameObject _parentPanel;
    [SerializeField] GameObject _buttonShowHelp;
    private int _curTutStep = 0;

    private bool _isWriting = false;

    public Action OnTutComplete;
    public Action onTutStart;
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
        onTutStart?.Invoke();
        ShowButtonHelp(false);
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        reset();
        StartSpeechWordByWord();
    }
    private void StartSpeechWordByWord()
    {
        StartCoroutine(DrawText(_curPart.tuts[_curTutStep].text));
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void onClick()
    {
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
                ShowTutor(false);
                ShowButtonHelp(true);
                OnTutComplete?.Invoke();
            }

        }

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
        if(enable) {
            StartTutorial();
        }
        else {
            UpdateText(""); // clear text
        }
    }
    public void ShowButtonHelp(bool e)
    {
        _buttonShowHelp.SetActive(e);
    }
    public void reset()
    {
        _curTutStep = 0;
    }
}
public enum TutoriaType
{
    MainMenu,
    Quiz,
    Pair,
    Run2D
}