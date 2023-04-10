using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCheckPoint : MonoBehaviour
{
    public System.Action<CheckPointType> OnClick;
    public CheckPointType checkPointType;
    public bool useClick = true;
    public void OnCheckPointClick()
    {
        OnClick.Invoke(checkPointType);
    }
    private void OnMouseDown()
    {
        if (useClick)
        {
            Debug.Log("Mouse Click Detected");
            OnCheckPointClick();
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