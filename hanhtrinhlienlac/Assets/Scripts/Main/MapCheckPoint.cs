using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCheckPoint : ObjectBase
{

    public CheckPointType checkPointType;
    public ObjectButton button;

    public GameObject highLight;
    // public MapIconStatus icon;
    private void Awake()
    {
        if (highLight != null)
        {
            highLight?.SetActive(false);
        }
    }
    void Start()
    {
        bool isUnlocked = UserInfo.GetInstance().IsLevelUnlocked(checkPointType);
        if (button != null)
        {
            button.type = checkPointType;
            button.useClick = isUnlocked;

        }

    }
}
[System.Serializable]
public enum CheckPointType
{
    NA,
    CHECK_POINT_1,
    CHECK_POINT_2,
    CHECK_POINT_3,
    CHECK_POINT_4,
}