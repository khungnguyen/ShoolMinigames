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
    private int _curTutStep = 0;

    private bool _isWriting = false;


    private TutPart _curPart;
    void Start()
    {
        _curPart = _data.getAllParts().Find(e => e.type == tutoriaType);
        StartSpeechWordByWord();
    }
    public void StartSpeechWordByWord()
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
            _curTutStep++;
            StartSpeechWordByWord();
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
            postfix = (s.Length == count - 1) ? "" : " _" ;
        }
            UpdateText(t + postfix);
        }
        _isWriting = false;
    }
}
public enum TutoriaType
{
    MainMenu,
    Quiz,
    Pair,
    Run2D
}