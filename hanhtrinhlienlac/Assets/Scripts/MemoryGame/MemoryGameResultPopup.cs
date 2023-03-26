using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameResultPopup : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI contentText;
    [SerializeField] private TMPro.TextMeshProUGUI extraInfoText;
    [SerializeField] private Image extraInfoImage;
    [SerializeField] private TMPro.TextMeshProUGUI btnNextText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show(bool finished, string extraText = null, Sprite extraImage = null)
    {
        gameObject.SetActive(true);
        contentText.text = "Chúc mừng!";
        if (finished) {
            btnNextText.text = "Hoàn thành";
        }
        //extra info
        extraInfoText.gameObject.SetActive(extraText != null);
        extraInfoText.text = extraText;
        extraInfoImage.gameObject.SetActive(extraImage != null);
        extraInfoImage.sprite = extraImage;
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
