using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameResultPopup : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI btnNextText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowHide(bool show, bool finished = false)
    {
        gameObject.SetActive(show);
        if (finished) {
            btnNextText.text = "Hoàn thành";
        }
    }
}
