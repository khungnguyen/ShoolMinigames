using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Texture2D cursor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     
     public void OnPointerEnter(PointerEventData data)
     {
            float xspot = cursor.width / 2;
            float yspot = cursor.height / 4;
            Vector2 hotSpot = new Vector2(xspot, yspot);
        #if UNITY_WEBGL
            Cursor.SetCursor(cursor, hotSpot, CursorMode.ForceSoftware);
        #else
            Cursor.SetCursor (cursor, hotSpot, CursorMode.Auto);
        #endif
     }
     
     public void OnPointerExit(PointerEventData data)
     {
        #if UNITY_WEBGL
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        #else
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        #endif
     }
}
