using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Memory game card", fileName = "New card")]
public class ScriptableCard : ScriptableObject
{
    public Sprite sprite;
    [TextArea(10, 20)]
    public string desc;
}
