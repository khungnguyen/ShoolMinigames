using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : ObjectBase
{
    public MarkedPoint revivePoint;
    private void OnTriggerEnter2D(Collider2D other) {
       // Debug.LogError("Obstacle Trigger " + other.tag);
        if(other.CompareTag("Buffalo")) {
            //TODO
        }
    }
}
