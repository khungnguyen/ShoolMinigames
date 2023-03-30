using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    public void setPosition(Vector2 v) {
            transform.position = v;
    }
    public Vector2 getPosition() {
        return transform.position;
    }
}
