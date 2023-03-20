using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform _target;


    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, _target.position,0.3f);
        transform.position = new Vector3(transform.position.x,transform.position.y,-10);
    }
}
