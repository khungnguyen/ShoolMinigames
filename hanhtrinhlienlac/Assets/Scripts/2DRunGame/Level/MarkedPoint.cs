using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class MarkedPoint : ObjectBase
{
    private void Awake()
    {
        var spr = GetComponentInChildren<SpriteRenderer>();
        if (spr!=null)
            spr.enabled = false;
    }
    public GameEnum.PointType type;

}
