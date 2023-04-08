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
    public Action<CheckPointType> OnCheckPointClickToPlayGame;

    private bool avoidUserClick = false;
    static int s_savePoint = -1;
    static CheckPointType s_saveCheckPoint;



    void Start()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }
        checkPoints.ForEach(e => e.OnClick += OnCheckPointSelected);
        _curScale = transform.localScale;
        OnResume();
        ChangeSkin(UserInfo.GetInstance().GetSkin());
    }
    void OnResume()
    {
        if (s_savePoint != -1 && s_saveCheckPoint != CheckPointType.NA)
        {
            StartAtPoint(s_savePoint);
            _checkPointType = s_saveCheckPoint;
        }
        if (UserInfo.GetInstance().IsPlayerCompleteRunGame())
        {
            OnCheckPointSelected(CheckPointType.CHECK_POINT_4);
            StartAtPoint(pathCreator.path.localPoints.Length -1);
        }
    }
    void StartAtPoint(int point)
    {

        // Debug.LogError("Num of Anchor" + pathCreator.bezierPath.NumAnchorPoints);
        // Debug.LogError("Num of NumPoints" + pathCreator.bezierPath.NumPoints);
        // Debug.LogError("Num of bezierPath.NumSegments" + pathCreator.bezierPath.NumSegments);
        transform.position = pathCreator.path.GetPoint(point);
        distanceTravelled = pathCreator.path.GetClosestTimeOnPath(transform.position) * pathCreator.path.length;
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
            //int curPoint = pathCreator.path.
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
            if (other.GetComponent<MapCheckPoint>().checkPointType == _checkPointType && avoidUserClick)
            {
                stop = true;
                var data = pathCreator.path.CalculateClosestPointOnPathData(transform.position);
                s_savePoint = data.previousIndex;
                SetAnimation(idle, true);
                Debug.Log("OnTriggerEnter2D");
                StartCoroutine(DelayGoToGame(_checkPointType));
            }
        }
    }
    public void OnCheckPointSelected(CheckPointType t)
    {

        if (!stop || avoidUserClick)
        {
            //char is moving, do not thing

        }
        else
        {
            avoidUserClick = true;
            Debug.Log("OnCheckPointSelected" + t);
            if (t == _checkPointType)
            {

                GoToGame(t);
                StopAllCoroutines();
            }
            else
            {
                stop = false;
                if ((int)t > (int)_checkPointType)
                {
                    _moveBack = false;
                    transform.localScale = _curScale * new Vector2(1, 1);
                }
                else
                {
                    _moveBack = true;
                    transform.localScale = _curScale * new Vector2(-1, 1);
                }
                s_saveCheckPoint = _checkPointType = t;
                SetAnimation(run, true);
            }
        }

    }
    public void ChangeSkin(string s)
    {
        spine.Skeleton.SetSkin(s);
        spine.Skeleton.SetSlotsToSetupPose();
        spine.LateUpdate();
    }
    public void SetAnimation(string anim, bool loop = false, Action completed = null)
    {
        var track = spine.AnimationState.SetAnimation(0, anim, loop);
        track.Complete += (t) =>
        {
            completed?.Invoke();
        };
    }
    IEnumerator DelayGoToGame(CheckPointType t)
    {
        yield return new WaitForSeconds(1);
        GoToGame(t);
    }
    private void GoToGame(CheckPointType t)
    {
        avoidUserClick = false;
        OnCheckPointClickToPlayGame?.Invoke(t);
        Debug.LogError("GotoGame");
    }

}
