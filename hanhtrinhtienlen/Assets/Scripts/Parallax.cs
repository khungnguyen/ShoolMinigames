using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public Vector2 movementScale = Vector2.one;

    private void Update()
    {
        if (cam != null)
        {
            transform.position = Vector2.Scale(cam.position, movementScale);
        }

    }
    public void setCameraTransform(Transform tr) {
      cam = tr;
    }
}
