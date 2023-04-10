using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCheckPoint : MonoBehaviour
{
    public System.Action<CheckPointType> OnClick;
    public CheckPointType checkPointType;
    public bool useClick = true;
    public void OnCheckPointClick()
    {
        OnClick.Invoke(checkPointType);
    }

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && useClick)
        {
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