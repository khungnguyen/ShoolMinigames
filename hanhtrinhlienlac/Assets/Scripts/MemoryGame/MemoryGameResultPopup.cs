using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameResultPopup : MonoBehaviour
{
    [SerializeField] private GameObject msgVuotQuaThuThach;
    [SerializeField] private GameObject msgMatThuFound;
    [SerializeField] private GameObject msgFishingRodFound;
    [SerializeField] private Image extraInfoImage;
    [SerializeField] private GameObject btnNext;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show(bool finished, Sprite extraImage = null, bool isFishingRod = false)
    {
        gameObject.SetActive(true);
        if (finished) {
            btnNext.SetActive(false);
        }

        //extra info
        if (extraImage) {
            extraInfoImage.gameObject.SetActive(true);
            extraInfoImage.sprite = extraImage;
            
            msgVuotQuaThuThach.SetActive(false);
            msgFishingRodFound.SetActive(isFishingRod);
            msgMatThuFound.SetActive(!isFishingRod);
        } else {
            extraInfoImage.gameObject.SetActive(false);

            msgVuotQuaThuThach.SetActive(true);
            msgFishingRodFound.SetActive(false);
            msgMatThuFound.SetActive(false);
        }
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
