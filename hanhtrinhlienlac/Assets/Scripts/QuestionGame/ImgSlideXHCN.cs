using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgSlideXHCN : MonoBehaviour
{
    [SerializeField] private int interval;
    [SerializeField] private Image image;
    [SerializeField] private Image imageLarge;
    [SerializeField] private GameObject maximize;

    private Sprite[] sprites = null;
    private float timeCount;
    private int spriteIdx = -1;

    public void SetSprites(Sprite[] arr)
    {
        gameObject.SetActive(true);
        sprites = arr;
        timeCount = interval;
        NextSprite();
    }

    public void OnMaximizeBtnClicked()
    {
        maximize.SetActive(true);
    }

    public void OnMaximizeEscBtnClicked()
    {
        maximize.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        maximize.SetActive(false);

        Scroring.Inst.AddCountableCB(() => !maximize.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        timeCount -= Time.deltaTime;
        if (timeCount <= 0) {
            timeCount = interval;
            NextSprite();
        }
    }

    void NextSprite()
    {
        if (sprites == null || sprites.Length <= 0) {
            return;
        }
        spriteIdx = (spriteIdx + 1) % sprites.Length;
        image.sprite = imageLarge.sprite = sprites[spriteIdx];
    }
}
