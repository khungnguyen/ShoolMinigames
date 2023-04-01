using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardContent : MonoBehaviour
{
    [SerializeField] private RectTransform itemsContainer;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(LeaderboardData data)
    {
        int idx = 0;
        for (; idx < data.list.Count; idx++) {
            var info = data.list[idx];
            GameObject go;
            if (idx < itemsContainer.childCount) {
                go = itemsContainer.GetChild(idx)?.gameObject;
                go.SetActive(true);
            } else {
                go = Instantiate(itemPrefab);
                go.transform.SetParent(itemsContainer);
            }
            var infoUI = go.GetComponent<LeaderboardItemUI>();
            infoUI.SetInfo(info);
        }

        for (; idx < itemsContainer.childCount; idx++) {
            itemsContainer.GetChild(idx).gameObject.SetActive(false);
        }

        //
        ScrollToTop();
    }

    public void ScrollToTop() 
    {
        StartCoroutine(SetScrollRectPosition(1));
    }

    public void ScrollToBottom() 
    {
        StartCoroutine(SetScrollRectPosition(0));
    }

    IEnumerator SetScrollRectPosition(float posY)
    {
        yield return null;
        scrollRect.verticalNormalizedPosition = posY;
    }
}
