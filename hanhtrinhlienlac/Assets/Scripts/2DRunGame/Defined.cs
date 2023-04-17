using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defined : MonoBehaviour
{
    public static string TAG_PLAYER ="Player";
    public static string TAG_COLLECTABLE ="Collectable";
    public static string TAG_OBSTACLE ="Obstacle";
    public static string TAG_ENDPOINT ="EndPoint";
    public static string TAG_BUFFALO ="Buffalo";
    public static string TAG_CHECKPOINT ="CheckPoint";
    public static string TAG_SOILDER ="Soilder";

    public  static int BONUS_SCORE = 1;
    public  static int BONUS_SCORE_BUFFALO = 1;

    public static string SAVE_KEY_CHAR ="SAVE_KEY_CHAR";

    public static bool CHEAT_BACK_AS_FINISHED = false;
     
}


public enum CollisionTags {
    
}