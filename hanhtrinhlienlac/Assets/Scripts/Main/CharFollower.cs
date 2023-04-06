using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using Spine.Unity;
using UnityEngine;

public class CharFollower : MonoBehaviour
{
    public SkeletonGraphic spine;
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public List<MapCheckPoint> checkPoints;
    public float speed = 5;

    [SpineAnimation]
    public string run;
    [SpineAnimation]
    public string idle;
    float distanceTravelled;
    private bool _moveBack = false;
    private CheckPointType _checkPointType;
    private Vector2 _curScale;

    void Start()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }
        checkPoints.ForEach(e => e.OnClick += OnCheckPointSelected);
        _curScale = transform.localScale;
    }
    bool stop = true;
    void Update()
    {
        if (pathCreator != null && !stop)
        {
            if (_moveBack)
            {
                distanceTravelled -= speed * Time.deltaTime;
            }
            else
            {
                distanceTravelled += speed * Time.deltaTime;
            }

            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            Quaternion r = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = Quaternion.Euler(r.x, r.y, r.z);
            // if(Vector2.Distance(transform.position,pathCreator.path.GetPoint(1))<=speed * Time.deltaTime) {
            //     stop = true;
            // }
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag(Defined.TAG_CHECKPOINT))
        {
            if (other.GetComponent<MapCheckPoint>().checkPointType == _checkPointType) 
            {
                stop = true;
                SetAnimation(idle);
            }
        }
    }
    public void OnCheckPointSelected(CheckPointType t)
    {
        Debug.Log("OnCheckPointSelected" + t);
        if(t == _checkPointType) return;
        stop = false;
        if((int)t >(int)_checkPointType) {
            _moveBack = false;
            transform.localScale=_curScale* new Vector2(1,1);
        }
        else {
            _moveBack = true;
            transform.localScale=_curScale* new Vector2(-1,1);
        }
        _checkPointType = t;
        SetAnimation(run);
    }
    public void ChangeSkin(string s)
    {
        spine.Skeleton.SetSkin(s);
        spine.Skeleton.SetSlotsToSetupPose();
        spine.LateUpdate();
    }
    public void SetAnimation(string anim) {
         spine.AnimationState.SetAnimation(0,anim, true);
    }

}
