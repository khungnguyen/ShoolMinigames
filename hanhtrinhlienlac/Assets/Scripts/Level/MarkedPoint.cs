using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class MarkedPoint : ObjectBase
{
    private void Awake() {
     //   GetComponent<SpriteRenderer>().enabled = false;
    }
 public GameEnum.PointType type;
  public GameEnum.LevelType nextLevel;
}
