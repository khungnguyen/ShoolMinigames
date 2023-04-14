using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectButton : MonoBehaviour
{
    public bool useClick = true;
    [System.Security.SecuritySafeCritical]
    public System.Action<object> OnClick;
    public GameObject textInfo;

    public object type;

    void Awake()
    {
        // if (sprRenderer != null)
        // {
        //     sprRenderer.color = new Color(1, 1, 1, 0);
        // }
        textInfo?.SetActive(false);
    }


    public void OnCheckPointClick()
    {
        OnClick.Invoke(type);
    }

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && useClick)
        {
            OnCheckPointClick();
        }
    }
    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && useClick)
        {
            // if (sprRenderer != null)
            // {
            //     sprRenderer.color =  new Color(1, 1, 1, 1);
        
            // }
            // textInfo?.SetActive(true);

        }
    }
    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && useClick)
        {
            // if (sprRenderer != null)
            // {
            //     sprRenderer.color = new Color(1, 1, 1, 0);
            // }
           // textInfo?.SetActive(false);

        }
    }
}
