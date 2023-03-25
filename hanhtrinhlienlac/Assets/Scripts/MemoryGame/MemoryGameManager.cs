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
        resutlPopup.ShowHide(false);

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

            Debug.Log(pair);
            var itemIdx = pair.A;
            var item = curLevelContainer.GetChild(itemIdx).GetComponent<MemoryGameItem>();
            item.SetData(itemIdx, sprite, OnItemSelected);
            item.SetState(false);
            if (!pair.Defective) {
                itemIdx = pair.B;
                item = curLevelContainer.GetChild(itemIdx).GetComponent<MemoryGameItem>();
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
            curLevelContainer.GetChild(itemIndex).GetComponent<MemoryGameItem>().SetState(true);
        }
        else if (itemIndex == curSelectedItemIndex) 
        {
            Debug.Log("Clicked on the last opened card. Face down the card");
            curLevelContainer.GetChild(itemIndex).GetComponent<MemoryGameItem>().SetState(false);
            curSelectedItemIndex = -1;
        }
        else
        {
            curLevelContainer.GetChild(itemIndex).GetComponent<MemoryGameItem>().SetState(true);

            var curSelectedPair = indexesInPairs.Find(p => p.Has(curSelectedItemIndex) && p.Has(itemIndex));
            if (curSelectedPair != null) 
            {
                Debug.Log("Math!");
                curLevelContainer.GetChild(itemIndex).GetComponent<MemoryGameItem>().OnMatched();
                curLevelContainer.GetChild(curSelectedItemIndex).GetComponent<MemoryGameItem>().OnMatched();
                // curLevelContainer.GetChild(itemIndex).GetComponent<MemoryGameItem>().ShowHide(false);
                // curLevelContainer.GetChild(curSelectedItemIndex).GetComponent<MemoryGameItem>().ShowHide(false);
                curSelectedItemIndex = -1;

                indexesInPairs.Remove(curSelectedPair);
                if (indexesInPairs.Count == 0)
                {
                    resutlPopup.ShowHide(true, IsLastLevel());
                }
                else if (indexesInPairs.Count == 1 && indexesInPairs[0].Defective)
                {
                    resutlPopup.ShowHide(true, IsLastLevel());
                }
            } 
            else 
            {
                Debug.Log("Not match, face down both of cards");
                curLevelContainer.GetChild(itemIndex).GetComponent<MemoryGameItem>().SetState(false, 0.5f);
                curLevelContainer.GetChild(curSelectedItemIndex).GetComponent<MemoryGameItem>().SetState(false, 0.5f);
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
}