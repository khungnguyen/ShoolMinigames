public class QuestionBank {
    private static QuestionBank _inst;
    public static QuestionBank Inst {
        get {
            return _inst;
        }
    }

    public string version;
    public Question[] questions;

    QuestionBank() {
        _inst = this;
    }
}