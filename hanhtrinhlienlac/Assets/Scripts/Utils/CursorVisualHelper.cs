using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorVisualHelper
{
    private static CursorVisualHelper inst;
    public static CursorVisualHelper Inst {
        get {
            if (inst == null) {
                inst = new CursorVisualHelper();
            }
            return inst;
        }
    }

    Texture2D cursor_default = null;
    Texture2D cursor_hover;
    Texture2D cursor_click;

    Vector2 cursor_hover_hotspot;
    Vector2 cursor_click_hotspot;

    CursorVisualHelper() {
        Init();
    }

    void Init() {
        cursor_hover = cursor_click = Resources.Load<Texture2D>("Texture/cursor_hover");
        cursor_hover_hotspot = cursor_click_hotspot = new Vector2(21, 0);
    }

    public void SetCursorDefault()
    {
        Cursor.SetCursor(cursor_default, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void SetCursorHover()
    {
        Cursor.SetCursor(cursor_hover, cursor_hover_hotspot, CursorMode.ForceSoftware);
    }

    public void SetCursorClick()
    {
        Cursor.SetCursor(cursor_click, cursor_click_hotspot, CursorMode.ForceSoftware);
    }
}
