using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryGameManager : MonoBehaviour
{
    [SerializeField] private Transform[] levelContainers;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Sprite fishingRodSprite;
    [SerializeField] private MemoryGameResultPopup resutlPopup;

    private int curLevelIdx = -1;
    private Transform curLevelContainer;

    private List<Pair<int>> indexesInPairs;

    private int curSelectedItemIndex = -1;


    // Start is called before the first frame update
    void Start()
    {
        foreach (var lc in levelContainers) {
            lc.gameObject.SetActive(false);
        }

        ShowNextLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonBackToMapClicked()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnButtonNextClicked() 
    {
        if (IsLastLevel()) {
            OnButtonBackToMapClicked();
            return;
        }
        curLevelContainer.gameObject.SetActive(false);
        ShowNextLevel();
    }

    public void ShowNextLevel()
    {
        resutlPopup.Hide();

        curLevelIdx++;
        Debug.Assert(curLevelIdx < levelContainers.Length);

        curLevelContainer = levelContainers[curLevelIdx];

        curLevelContainer.gameObject.SetActive(true);
        //Setup level
        indexesInPairs = RandomPairsFromRange(0, curLevelContainer.childCount);
        var spriteIndexes = Enumerable.Range(0, sprites.Length).ToList();
        
        Debug.Assert(spriteIndexes.Count >= indexesInPairs.Count);

        foreach (var pair in indexesInPairs) {
            //random sprite
            var idx = UnityEngine.Random.Range(0, spriteIndexes.Count - 1);
            var spriteIdx = spriteIndexes[idx];
            spriteIndexes.RemoveAt(idx);
            var sprite = sprites[spriteIdx];

            if (IsLastLevel() && pair.Defective) {
                sprite = fishingRodSprite;
            }

            Debug.Log(pair);
            var itemIdx = pair.A;
            var item = GetItemByIndex(itemIdx);
            item.SetData(itemIdx, sprite, OnItemSelected);
            item.SetState(false);
            if (!pair.Defective) {
                itemIdx = pair.B;
                item = GetItemByIndex(itemIdx);
                item.SetData(itemIdx, sprite, OnItemSelected);
                item.SetState(false);
            }
        }
    }

    public void OnItemSelected(int itemIndex)
    {
        // Debug.Log("On selected item " + itemIndex);
        if (curSelectedItemIndex < 0) 
        {
            Debug.Log("No cards is turn over, just turn over the selected one");
            curSelectedItemIndex = itemIndex;
            GetItemByIndex(itemIndex).SetState(true);
        }
        else if (itemIndex == curSelectedItemIndex) 
        {
            Debug.Log("Clicked on the last opened card. Face down the card");
            GetItemByIndex(itemIndex).SetState(false);
            curSelectedItemIndex = -1;
        }
        else
        {
            GetItemByIndex(itemIndex).SetState(true);

            var curSelectedPair = indexesInPairs.Find(p => p.Has(curSelectedItemIndex) && p.Has(itemIndex));
            if (curSelectedPair != null) 
            {
                Debug.Log("Matched!");
                GetItemByIndex(itemIndex).OnMatched();
                GetItemByIndex(curSelectedItemIndex).OnMatched();
                // GetItemByIndex(itemIndex).ShowHide(false);
                // GetItemByIndex(curSelectedItemIndex).ShowHide(false);
                curSelectedItemIndex = -1;

                indexesInPairs.Remove(curSelectedPair);

                if (indexesInPairs.Count == 0)
                {
                    StartCoroutine(ShowResultPopup(0.5f, null, null));
                }
                else if (indexesInPairs.Count == 1 && indexesInPairs[0].Defective)
                {
                    var item = GetItemByIndex(indexesInPairs[0].A);
                    item.SetState(true, 1f);
                    string extraText = IsLastLevel() ? "Công cụ tìm được" : "Mật thư tìm được";
                    Sprite extraImage = item.GetSprite();
                    StartCoroutine(ShowResultPopup(2f, extraText, extraImage));
                }
            } 
            else 
            {
                Debug.Log("Not match, face down both of cards");
                GetItemByIndex(itemIndex).SetState(false, 0.5f);
                GetItemByIndex(curSelectedItemIndex).SetState(false, 0.5f);
                curSelectedItemIndex = -1;
            }
        }
    }

    private List<Pair<int>> RandomPairsFromRange(int start, int count)
    {
        var listSrc = Enumerable.Range(start, count).ToList();
        var listPair = new List<Pair<int>>();

        while (listSrc.Count > 1) {
            var idx = UnityEngine.Random.Range(0, listSrc.Count);
            var a = listSrc[idx];
            listSrc.RemoveAt(idx);

            idx = UnityEngine.Random.Range(0, listSrc.Count);
            var b = listSrc[idx];
            listSrc.RemoveAt(idx);

            listPair.Add(new Pair<int>(a, b));
        }

        if (listSrc.Count > 0) {
            listPair.Add(new Pair<int>(listSrc[0])); // The last item from listSrc
        }

        return listPair;
    }

    private bool IsLastLevel()
    {
        return curLevelContainer != null && curLevelContainer.childCount >= 25;
    }

    private MemoryGameItem GetItemByIndex(int idx)
    {
        return curLevelContainer.GetChild(idx).GetComponent<MemoryGameItem>();
    }

    private IEnumerator ShowResultPopup(float delaySec, string extraText = null, Sprite extraImage = null)
    {
        yield return new WaitForSeconds(delaySec);
        resutlPopup.Show(IsLastLevel(), extraText, extraImage);
    }
}
