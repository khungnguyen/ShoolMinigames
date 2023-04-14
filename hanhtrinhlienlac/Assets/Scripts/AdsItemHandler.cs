using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdsItemHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string url;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Application.OpenURL(url);
    }
}
