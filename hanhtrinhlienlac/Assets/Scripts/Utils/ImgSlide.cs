using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgSlide : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private int interval;

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

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
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
        image.sprite = sprites[spriteIdx];
    }
}
