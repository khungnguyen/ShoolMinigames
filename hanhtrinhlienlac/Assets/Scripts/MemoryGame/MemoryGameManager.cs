using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryGameManager : MonoBehaviour
{
    [Serializable]
    struct AudioClips
    {
        public AudioClip bgm;
        public AudioClip nextGameSFX;
        public AudioClip gameSolvedSFX;
        public AudioClip clickSFX;
        public AudioClip scoringSFX;
        public AudioClip wrongSFX;
        public AudioClip levelFinishedSFX;
    }
    [SerializeField] private Transform[] levelContainers;
    [SerializeField] private ScriptableCard[] scriptableCards;
    [SerializeField] private ScriptableCard matThuCard;
    [SerializeField] private ScriptableCard fishingRodCard;
    [SerializeField] private TMPro.TextMeshProUGUI cardInfoTMP;
    [SerializeField] private MemoryGameResultPopup resutlPopup;
    [SerializeField] private RewardUI rewardUI;
    [SerializeField] private int scorePerMatchedPair;
    [SerializeField] private TutorManager tutorComp;
    [SerializeField] private SoundManager soundMgr;
    [SerializeField] private AudioClips audioClips;

    [SerializeField] private List<Sprite> secretItemSprs;

    private int curLevelIdx = -1;
    private Transform curLevelContainer;
    private MemoryGameItem curSelectedItem;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var lc in levelContainers)
        {
            lc.gameObject.SetActive(false);
        }

        ShowNextLevel();
        Scroring.Inst.Pause();

        tutorComp.OnTutComplete += (t) =>
        {
            Scroring.Inst.StartOrResume();
            soundMgr.PlaySfx(audioClips.nextGameSFX);
        };

        tutorComp.OnTutStart += (t) =>
        {
            Scroring.Inst.Pause();
        };

        StartCoroutine(PlayBGMDelay(audioClips.bgm, true));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonBackToMapClicked()
    {
        soundMgr.PlaySfx(audioClips.clickSFX, false, 1);
        SceneManager.LoadScene("Main");
    }

    public void OnButtonNextClicked()
    {
        if (IsLastLevel())
        {
            OnButtonBackToMapClicked();
            return;
        }
        curLevelContainer.gameObject.SetActive(false);
        ShowNextLevel();
    }

    public void ShowNextLevel()
    {
        Scroring.Inst.StartOrResume();
        if (curLevelIdx >= 0)
        {
            soundMgr.PlaySfx(audioClips.nextGameSFX);
        }
        resutlPopup.Hide();

        curLevelIdx++;
        Debug.Assert(curLevelIdx < levelContainers.Length);

        curLevelContainer = levelContainers[curLevelIdx];

        curLevelContainer.gameObject.SetActive(true);
        //Setup level
        curSelectedItem = null;
        var indexesInPairs = RandomPairsFromRange(0, curLevelContainer.childCount);
        var cardIndexes = Enumerable.Range(0, scriptableCards.Length).ToList();

        Debug.Assert(cardIndexes.Count >= indexesInPairs.FindAll(pair => !pair.Defective).Count);

        foreach (var pair in indexesInPairs)
        {
            ScriptableCard data = null;
            if (pair.Defective) {
                data = IsLastLevel() ? fishingRodCard : matThuCard;
            } else {
                //random card data
                var idx = UnityEngine.Random.Range(0, cardIndexes.Count - 1);
                var cardIdx = cardIndexes[idx];
                cardIndexes.RemoveAt(idx);
                data = scriptableCards[cardIdx];
            }

            Debug.Log(pair);

            var itemIdx = pair.A;
            var item = GetItemByIndex(itemIdx);
            item.SetData(data, OnItemSelected);
            item.SetState(false);
            if (!pair.Defective)
            {
                itemIdx = pair.B;
                item = GetItemByIndex(itemIdx);
                item.SetData(data, OnItemSelected);
                item.SetState(false);
            }
        }
    }

    public void OnItemSelected(MemoryGameItem item)
    {
        soundMgr.PlaySfx(audioClips.clickSFX);
        if (curSelectedItem == null)
        {
            Debug.Log("No cards is turn over, just turn over the selected one");
            curSelectedItem = item;
            item.SetState(true);
            StartCoroutine(ShowCardInfo(item.CardData, 0.5f));
        }
        else if (item == curSelectedItem)
        {
            Debug.Log("Clicked on the last opened card. Face down the card");
            item.SetState(false);
            curSelectedItem = null;
            StartCoroutine(ShowCardInfo(null, 0f));
        }
        else
        {
            item.SetState(true);

            if (item.CardData == curSelectedItem.CardData)
            {
                Debug.Log("Matched!");
                Scroring.Inst.AddRemainingTimeScore(scorePerMatchedPair);
                soundMgr.PlaySfx(audioClips.scoringSFX, false, 1, 100, 0.5f);

                item.OnMatched();
                curSelectedItem.OnMatched();
                item.ShowHide(false, false, 1f);
                curSelectedItem.ShowHide(false, false, 1f);
                StartCoroutine(ShowCardInfo(null, 1f));

                curSelectedItem = null;

                var remainingItems = GetRemainingItems();

                if (remainingItems.Count <= 1)
                {
                    // Grid was solved
                    Scroring.Inst.Pause();
                    if (remainingItems.Count == 0)
                    {
                        StartCoroutine(ShowResultPopup(0.5f, null));
                    }
                    else if (remainingItems.Count == 1)
                    {
                        var lastItem = remainingItems[0];
                        lastItem.SetState(true, 1f);
                        StartCoroutine(ShowCardInfo(lastItem.CardData, 1f));
                        soundMgr.PlaySfx(audioClips.clickSFX, false, 0, 100, 1f);
                        StartCoroutine(ShowResultPopup(2f, lastItem.CardData.sprite, IsLastLevel()));
                    }
                    // if (finishAGame)
                    // {
                    //     float waitingTime = remainingItems.Count == 1 ? 2f : 0.5f;

                    //     if (curLevelIdx < 2)
                    //     {

                    //         StartCoroutine(ShowResultPopup(waitingTime, secretItemSprs[curLevelIdx]));
                    //     }
                    //     else if (curLevelIdx == 2)
                    //     {
                    //         soundMgr.PlaySfx(audioClips.clickSFX, false, 0, 100, 1f);
                    //         StartCoroutine(ShowResultPopup(waitingTime, null, IsLastLevel()));
                    //     }
                    // }
                }
            }
            else
            {
                Debug.Log("Not match, face down both of cards");
                soundMgr.PlaySfx(audioClips.wrongSFX, false, 1, 100, 0.5f);
                item.SetState(false, 0.5f);
                curSelectedItem.SetState(false, 0.5f);
                curSelectedItem = null;
                StartCoroutine(ShowCardInfo(null, 1f));
            }
        }
    }

    private List<Pair<int>> RandomPairsFromRange(int start, int count)
    {
        var listSrc = Enumerable.Range(start, count).ToList();
        var listPair = new List<Pair<int>>();

        while (listSrc.Count > 1)
        {
            var idx = UnityEngine.Random.Range(0, listSrc.Count);
            var a = listSrc[idx];
            listSrc.RemoveAt(idx);

            idx = UnityEngine.Random.Range(0, listSrc.Count);
            var b = listSrc[idx];
            listSrc.RemoveAt(idx);

            listPair.Add(new Pair<int>(a, b));
        }

        if (listSrc.Count > 0)
        {
            listPair.Add(new Pair<int>(listSrc[0])); // The last item from listSrc
        }

        return listPair;
    }

    private IEnumerator PlayBGMDelay(AudioClip ac, bool loop)
    {
        yield return new WaitForEndOfFrame();
        soundMgr.PlayBGM(ac, loop);
    }

    private IEnumerator ShowCardInfo(ScriptableCard card, float delay)
    {
        yield return new WaitForSeconds(delay);
        cardInfoTMP.text = card ? card.desc : "";
    }

    private List<MemoryGameItem> GetRemainingItems()
    {
        var list = new List<MemoryGameItem>();
        for (int i = curLevelContainer.childCount - 1; i >= 0; i--)
        {
            var item = curLevelContainer.GetChild(i).GetComponent<MemoryGameItem>();
            if (!item.Matched)
            {
                list.Add(item);
            }
        }
        return list;
    }

    private bool IsLastLevel()
    {
        return curLevelContainer != null && curLevelContainer.childCount >= 25;
    }

    private MemoryGameItem GetItemByIndex(int idx)
    {
        return curLevelContainer.GetChild(idx).GetComponent<MemoryGameItem>();
    }

    private IEnumerator ShowResultPopup(float delaySec, Sprite extraImage = null, bool isFinishRod = false, bool Anim = false)
    {
        yield return new WaitForSeconds(delaySec);
        soundMgr.PlaySfx(audioClips.gameSolvedSFX, false, 1);
        resutlPopup.Show(IsLastLevel(), extraImage, isFinishRod);

        if (IsLastLevel())
        {
            resutlPopup.Show(IsLastLevel(), extraImage, isFinishRod);
            yield return new WaitForSeconds(3);
            resutlPopup.Show(IsLastLevel(), extraImage, isFinishRod,true);
            StartCoroutine(ShowRewardUI(6));
            UserInfo.GetInstance().SetUnlockLevel(CheckPointType.CHECK_POINT_3);
        }
    }
    private IEnumerator ShowRewardUI(float delay)
    {
        yield return new WaitForSeconds(delay);
        soundMgr.PlaySfx(audioClips.levelFinishedSFX);
        rewardUI.Show(Scroring.Inst.TotalScore.ToString(), OnButtonBackToMapClicked);
    }
}
