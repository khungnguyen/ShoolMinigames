using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardContent : MonoBehaviour
{
    [SerializeField] private RectTransform itemsContainer;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject localPlayerItemPrefab;
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
        int uiDestroyedCount = 0;
        int idx = 0;
        for (; idx < data.list.Count; idx++) {
            var info = data.list[idx];
            bool isLocalPlayer = info.playerId == "tuiot";

            GameObject go = null;
            while (idx + uiDestroyedCount < itemsContainer.childCount) {
                var _go = itemsContainer.GetChild(idx + uiDestroyedCount).gameObject;
                if (_go.GetComponent<LeaderboardItemUI>().IsForLocalPlayer == isLocalPlayer) {
                    go = _go;
                    go.SetActive(true);
                    break;
                } else {
                    Object.Destroy(_go);
                    uiDestroyedCount++;
                }
            }

            if (go == null) {
                go = Instantiate(isLocalPlayer ? localPlayerItemPrefab : itemPrefab);
                go.transform.SetParent(itemsContainer);
            }
            var infoUI = go.GetComponent<LeaderboardItemUI>();
            infoUI.SetInfo(info);
        }

        int uiIdx = idx + uiDestroyedCount;
        for (; uiIdx < itemsContainer.childCount; uiIdx++) {
            itemsContainer.GetChild(uiIdx).gameObject.SetActive(false);
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
