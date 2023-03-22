using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBankLoader : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;

    private void Awake() 
    {
        LoadJson();
    }

    // Start is called before the first frame update
    void Start()
    {
        // LoadJson();
    }

    private void LoadJson() {
        QuestionBank questionBank = JsonUtility.FromJson<QuestionBank>(jsonFile.text);
#if UNITY_EDITOR
        questionBank.DebugInfo();
#endif
    }
}
