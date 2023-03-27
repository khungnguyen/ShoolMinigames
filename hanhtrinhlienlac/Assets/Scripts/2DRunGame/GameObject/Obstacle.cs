using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : ObjectBase
{
    public ParticleSystem dustVfx;
    public MarkedPoint revivePoint;

    private void Awake() {
        //dustVfx.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other) {
       // Debug.LogError("Obstacle Trigger " + other.tag);
        // if(other.CompareTag("Buffalo")) {
        //     //TODO
        //     if(objectType!= GameEnum.ObjectType.WATER) {
        //        var par = Instantiate(dustVfx,transform.position,transform.rotation);
        //        Destroy(par,2f);
        //     }

        //    Destroy(gameObject);
        // }
    }
}
