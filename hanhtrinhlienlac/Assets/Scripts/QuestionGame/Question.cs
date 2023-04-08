[System.Serializable]
public class Question
{
    [System.Serializable]
    public class Answer {
        public bool value;
        public string text;
    }

    public bool allowShuffle;
    public bool anwserByImage;
    public int score;
    public string text;
    public Question.Answer[] answers;
    public string moreInfoURL;
    public string note;
}
