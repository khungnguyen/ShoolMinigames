using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MousePointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool cursorChangable = true;
    protected bool CursorChangable {
        get {
            return cursorChangable;
        }

        set {
            cursorChangable = value;
            if (!value) {
                CursorVisualHelper.Inst.SetCursorDefault();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CursorChangable) return;
        CursorVisualHelper.Inst.SetCursorHover();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CursorChangable) return;
        if (eventData.fullyExited)
        {
            CursorVisualHelper.Inst.SetCursorDefault();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CursorChangable) return;
        CursorVisualHelper.Inst.SetCursorClick();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CursorChangable) return;
        // CursorVisualHelper.Inst.SetCursorDefault();
    }
}
