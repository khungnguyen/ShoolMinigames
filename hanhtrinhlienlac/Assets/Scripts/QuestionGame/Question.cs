[System.Serializable]
public class Question
{
    [System.Serializable]
    public class Answer {
        public bool value;
        public string text;
    }

    public bool allowShuffle;
    public string text;
    public Question.Answer[] answers;
}
