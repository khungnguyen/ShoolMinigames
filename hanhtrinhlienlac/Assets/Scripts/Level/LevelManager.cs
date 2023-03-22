using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<LevelInfo> _levels;

    public LevelInfo FindLevel(GameEnum.LevelType level) {
       return _levels.Find(e=>e.getLevelType() == level);
    }

}