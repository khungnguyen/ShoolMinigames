using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultTextTMPro;
    [SerializeField] private Button btnNext;
    [SerializeField] private string[] correctTexts;
    [SerializeField] private string[] incorrectTexts;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show(bool result)
    {
        gameObject.SetActive(true);
        var texts = result ? correctTexts : incorrectTexts;
        var idx = Random.Range(0, texts.Length - 1);
        resultTextTMPro.text = texts[idx];
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
