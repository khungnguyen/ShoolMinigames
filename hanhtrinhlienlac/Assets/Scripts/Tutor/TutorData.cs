using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorData : MonoBehaviour
{
    // Start is called before the first frame update
    public List<TutPart> tutData;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public List<TutPart> getAllParts() {
        return tutData;
    }
}
[System.Serializable]
public struct TutPart
{
    public TutoriaType type;
    public List<TutItem> tuts;
}
[System.Serializable]
public struct TutItem
{
    public string text;
    public AudioClip audio;
}

