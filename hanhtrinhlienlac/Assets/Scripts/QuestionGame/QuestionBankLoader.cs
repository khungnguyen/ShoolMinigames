using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBankLoader : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;

    // Start is called before the first frame update
    void Start()
    {
        LoadJson();
    }

    private void LoadJson() {
        QuestionBank questionBank = JsonUtility.FromJson<QuestionBank>(jsonFile.text);
        Debug.Log("Question bank version: " + questionBank.version);
    }
}
