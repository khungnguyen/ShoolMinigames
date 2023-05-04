using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Question", fileName = "New question")]
public class ScriptableQuestion: ScriptableObject
{
    [System.Serializable]
    public class Answer {
        private static string[] ids = new[] {"A", "B", "C", "D", "E", "G", "H", "I", "K"};
        public string id = null;
        [TextArea(1, 5)]
        public string text = null;
        public Sprite img = null;
        public bool value = false;

        public void SetIndex(int index)
        {
            Debug.Assert(index < ids.Length);
            id = ids[index];
        }
    }

    public bool allowShuffle = true;
    public bool anwserByImage = false;
    public int score = 10;
    [TextArea(2, 5)]
    public string text;

    public List<Answer> answers;
    public int numMixedAnswer = 0;

    [TextArea(2, 5)]
    public string extraText;
    public Sprite[] extraImages;
    public string moreInfoURL;

    public void Init()
    {
        ShuffleAnswers();
        DetermineAnswersId();

        var correctAnswers = answers.FindAll(a => a.value);
        Debug.Assert(correctAnswers.Count > 0);

        if (correctAnswers.Count > 1) {
            Debug.Assert(numMixedAnswer > 0);
            // Generate mixed answers 
            Debug.Log("ScriptableQuestion: generate mixed answers");
            int correctId = Random.Range(0, numMixedAnswer - 1);
            for (int i = 0; i < numMixedAnswer; i++) {
                bool isCorrect = i == correctId;
                List<Answer> mixedList;

                if (isCorrect) {
                    mixedList = correctAnswers;
                } else {
                    mixedList = new List<Answer>();
                    int ii = Random.Range(0, answers.Count - 1 - correctAnswers.Count);
                    mixedList.Add(answers[ii++]);
                    int count = correctAnswers.Count - 1;
                    for (; ii < answers.Count; ii++) {
                        var a = answers[ii];
                        if (a.value && mixedList.Find(a => !a.value) == null) {
                            continue;
                        }
                        mixedList.Add(a);
                        count--;
                        if (count <= 0) {
                            break;
                        }
                    }
                }

                string text = "" + mixedList[0].id;
                for (int ii = 1; ii < mixedList.Count; ii++) {
                    text += " & " + mixedList[ii].id;
                }

                Answer answer = new Answer() {
                    text = text,
                    value = isCorrect
                };
                answer.SetIndex(answers.Count);
                answers.Add(answer);
            }

            foreach (var answer in correctAnswers) {
                answer.value = false;
            }
        }
    }

    void ShuffleAnswers()
    {
        Utils.Shuffle<Answer>(answers);
    }

    void DetermineAnswersId()
    {
        int i = 0;
        foreach (var answer in answers) {
            answer.SetIndex(i++);
        }
    }
}
