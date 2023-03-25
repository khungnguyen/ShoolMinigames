using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionGameResultPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultTextTMPro;
    [SerializeField] private TextMeshProUGUI btnNextText;
    [SerializeField] private string[] correctTexts;
    [SerializeField] private string[] incorrectTexts;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show(bool result, bool finished = false)
    {
        gameObject.SetActive(true);
        var texts = result ? correctTexts : incorrectTexts;
        var idx = Random.Range(0, texts.Length - 1);
        resultTextTMPro.text = texts[idx];

        if (finished) {
            btnNextText.text = "Hoàn thành";
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
