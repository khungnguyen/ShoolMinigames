using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CharFollower _char;
    void Start()
    {
        _char.OnCheckPointClickToPlayGame += OnSelectedLevel;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnLevelClicked(string sceneName)
    {
        Debug.Log("Select level " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    void OnDestroy()
    {
        _char.OnCheckPointClickToPlayGame -= OnSelectedLevel;
    }
    public void OnSelectedLevel(CheckPointType t)
    {
        string level = "";
        switch (t)
        {
            case CheckPointType.CHECK_POINT_1:
                {
                    level = "Level_1";
                    break;
                }
            case CheckPointType.CHECK_POINT_2:
                {
                    level = "Level_2";
                    break;
                }
            case CheckPointType.CHECK_POINT_3:
                {
                    level = "Run2D";
                    break;
                }
        }
        if(level.Length > 0) {
            SceneManager.LoadScene(level);
        }
    }
}
