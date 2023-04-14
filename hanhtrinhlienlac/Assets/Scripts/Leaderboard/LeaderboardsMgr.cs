using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ELeaderboardId {
    FINAL,
    QUESTION_GAME,
    MEMORY_GAME,
    ACTION_GAME
}

public class LeaderboardsMgr : MonoBehaviour
{
    [Serializable] struct AudioClips {
        public AudioClip bgm;
        public AudioClip clickSFX;
    }
    private static LeaderboardsMgr _inst;
    public static LeaderboardsMgr Inst { get => _inst; }

    [SerializeField] private List<LeaderboardTab> tabs;
    [SerializeField] private LeaderboardContent content;

    [SerializeField] private SoundManager soundMgr;
    [SerializeField] private AudioClips audioClips;
    private LeaderboardTab curSelectedTab;

    void Awake() 
    {
        _inst = this;
    }

    void OnDestroy()
    {
        _inst = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        var defaultSelectedItem = tabs.Find(t => t.Id == ELeaderboardId.QUESTION_GAME);
        defaultSelectedItem.OnSelected();        
        StartCoroutine(PlayBGMDelay(audioClips.bgm, true));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("Main");
        soundMgr.PlaySfx(audioClips.clickSFX);
    }

    public void OnTabSelected(LeaderboardTab item)
    {
        curSelectedTab?.OnDeselected();
        curSelectedTab = item;
        content.SetData(item.Data);
        int idx = 0;
        for (int i = 0; i < tabs.Count; i++) {
            var tab = tabs[i];
            if (tab == curSelectedTab) {
                break;
            }
            tab.transform.SetSiblingIndex(idx++);
        }
        for (int i = tabs.Count - 1; i > 0; i--) {
            var tab = tabs[i];
            tab.transform.SetSiblingIndex(idx++);
            if (tab == curSelectedTab) {
                break;
            }
        }
        StartCoroutine(PlaySFXDelay(audioClips.clickSFX));
        
        // for (int i = 0; i < tabs.Count; i++) {
        //     tabs[i].transform.SetSiblingIndex(tabs.Count - 1 - i);
        // }
    }

    public void OnTabDataFetchingFinished(LeaderboardTab tab)
    {
        if (curSelectedTab == tab) {
            content.SetData(tab.Data);
        }
    }

    private IEnumerator PlayBGMDelay(AudioClip ac, bool loop)
    {
        yield return new WaitForEndOfFrame();
        soundMgr.PlayBGM(ac, loop);
    }

    private IEnumerator PlaySFXDelay(AudioClip ac)
    {
        //Thanh.To cheat delay to avoid error at first frame
        yield return new WaitForEndOfFrame();
        soundMgr.PlaySfx(ac);
    }
}
