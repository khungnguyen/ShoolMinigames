using UnityEngine;

[System.Serializable]
public class QuestionBank {
    private static QuestionBank _inst;
    private static QuestionBank Inst {
        get {
            return _inst;
        }
    }

    public string version;
    public Question[] questions;

    QuestionBank() {
        _inst = this;
    }

    public void DebugInfo() {
        Debug.Log("[QuestionBank] version: " + version);
        Debug.Log("[QuestionBank] questions count: " + questions.Length);
    }
}