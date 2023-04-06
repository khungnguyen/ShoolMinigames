using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCheckPoint : MonoBehaviour
{
    public System.Action<CheckPointType> OnClick;
    public CheckPointType checkPointType;
    public void OnCheckPointClick()
    {
        OnClick.Invoke(checkPointType);
    }
}
[System.Serializable]
public enum CheckPointType
{
    NA,
    CHECK_POINT_1,
    CHECK_POINT_2,
    CHECK_POINT_3,
}