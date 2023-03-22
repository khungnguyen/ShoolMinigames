using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnswerItemUI : MonoBehaviour
{
    private static string[] ids = new[] {"A", "B", "C", "D", "E", "G", "H", "I", "K"};
    [SerializeField] private TextMeshProUGUI idTMPro;
    [SerializeField] private TextMeshProUGUI textTMPro;
    // Start is called before the first frame update

    private int index;

    void Start()
    {
        
    }

    public void SetData(int index, string text) {
        this.index = index;
        idTMPro.text = ids[index];
        textTMPro.text = text;
    }
}
