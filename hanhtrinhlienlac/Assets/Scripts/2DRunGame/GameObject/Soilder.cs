using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Soilder : MonoBehaviour
{
    [SerializeField] SkeletonAnimation _spine;
    [SpineAnimation] public string _idle;
    [SpineAnimation] public string _bravo;

    private void Start()
    {
      //  ChangeAnim(_idle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Defined.TAG_PLAYER))
        {
            ChangeAnim(_bravo,true);

        }
    }
    private void ChangeAnim(string s,bool loop =false)
    {
        _spine.AnimationState.SetAnimation(0, s, loop);
    }

}
