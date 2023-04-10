using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Action OnClick;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                Debug.Log("Button Clicked");
                OnClick?.Invoke();
            }
        }
    }
}
