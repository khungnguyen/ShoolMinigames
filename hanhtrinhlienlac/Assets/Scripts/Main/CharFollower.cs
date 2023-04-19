using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using Spine.Unity;
using UnityEngine;

public class CharFollower : MonoBehaviour
{
    public SkeletonAnimation spine;
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public List<MapCheckPoint> checkPoints;
    public float speed = 5;

    [SpineAnimation]
    public string run;
    [SpineAnimation]
    public string idle;
    float distanceTraveled;
    private bool _moveBack = false;
    private CheckPointType _curCheckPoint;
    private Vector2 _curScale;
    public Action<CheckPointType> OnCheckPointClickToPlayGame;

    private bool avoidUserClick = false;
    static int s_savePoint = -1;
    static CheckPointType s_saveCheckPoint;

    private bool _useTeleportToHigherCheckPoint = true;

    private bool _useAutoMove = false;

    private bool _hasTeleported = false;

    void Start()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }
        checkPoints.ForEach(e =>
        {
            if (e.button != null)
            {
                e.button.OnClick += OnCheckPointSelected;
            }

        });
        _curScale = transform.localScale;
        OnResume();
        ChangeSkin(UserInfo.GetInstance().GetSkin());

    }
    void OnResume()
    {

        if (UserInfo.GetInstance().IsPlayerCompleteRunGame() && s_saveCheckPoint == CheckPointType.CHECK_POINT_3)
        {
            // teleport to checkock Point 4
            StartAtPoint(pathCreator.path.localPoints.Length - 1);
            _curCheckPoint = CheckPointType.CHECK_POINT_4;
            UserInfo.GetInstance().SetCompletedRunGame(false);
        }
        else if (s_savePoint != -1 && s_saveCheckPoint != CheckPointType.NA)
        {
            StartAtPoint(s_savePoint);

            _curCheckPoint = s_saveCheckPoint;
            if (_useAutoMove)
            {

                //trigger auto move
                // when user not complete all level
                if (_curCheckPoint < CheckPointType.CHECK_POINT_3 && !UserInfo.GetInstance().IsLastLevelUnlocked())
                {
                    OnCheckPointSelected(_curCheckPoint + 1);
                }
            }
            else
            {
                if (_curCheckPoint <= CheckPointType.CHECK_POINT_3)
                {
                    var useHasFinishCurGame = UserInfo.GetInstance().IsLevelUnlocked(_curCheckPoint + 1);
                    // incase user is back but not complete the game
                    if (!useHasFinishCurGame)
                    {
                        _hasTeleported = true;
                    }
                }

            }

        }
        else
        {
            if (_useTeleportToHigherCheckPoint)
            {
                int CalculatePoint(CheckPointType l)
                {
                    var checkpoint = checkPoints.Find(e => e.checkPointType == l);
                    var circleCollider = checkpoint.GetComponent<CircleCollider2D>();
                    if (circleCollider)
                    {
                        var pos = new Vector2(circleCollider.transform.position.x, circleCollider.transform.position.y) + circleCollider.offset + Vector2.left * circleCollider.radius;
                        var point = pathCreator.path.CalculateClosestPointOnPathData(pos);
                        return point.previousIndex;
                    }
                    else
                    {
                        var point = pathCreator.path.CalculateClosestPointOnPathData(checkpoint.getPosition());
                        return point.previousIndex;
                    }

                }
                if (UserInfo.GetInstance().IsLastLevelUnlocked())
                {
                    // do nothing
                    // player should be at start place
                }
                else if (UserInfo.GetInstance().IsLevelUnlocked(CheckPointType.CHECK_POINT_3))
                {
                    StartAtPoint(CalculatePoint(CheckPointType.CHECK_POINT_3));
                    s_saveCheckPoint = _curCheckPoint = CheckPointType.CHECK_POINT_3;
                    _hasTeleported = true;
                }
                else if (UserInfo.GetInstance().IsLevelUnlocked(CheckPointType.CHECK_POINT_2))
                {

                    StartAtPoint(CalculatePoint(CheckPointType.CHECK_POINT_2));
                    s_saveCheckPoint = _curCheckPoint = CheckPointType.CHECK_POINT_2;
                    _hasTeleported = true;
                }
                else if (UserInfo.GetInstance().IsLevelUnlocked(CheckPointType.CHECK_POINT_1))
                {
                    // do nothing
                }


            }
        }
        HandleHighLightCheckPoint();
    }
    private void HandleHighLightCheckPoint()
    {
        checkPoints.ForEach(e =>
        {
            if (e.highLight != null)
            {
                var unlock = UserInfo.GetInstance().IsLevelUnlocked(e.checkPointType);
                if (unlock && (
                _curCheckPoint + 1 == e.checkPointType && !_useAutoMove // NEXT POINT SHOULD BE USE HIGHLIGHT AFTER USER CLICKS
                || _curCheckPoint == e.checkPointType && _useAutoMove // POINT IS ALL READY SET BY CODE
                || _curCheckPoint == CheckPointType.NA
                || UserInfo.GetInstance().IsLastLevelUnlocked()
                || _hasTeleported && _curCheckPoint == e.checkPointType
                ))
                {
                    e.highLight.SetActive(true);
                }
                else
                {
                    e.highLight.SetActive(false);
                }
            }
        });
        _hasTeleported = false;
    }
    void StartAtPoint(int point)
    {
        if (point < 0) return;
        var p = point < (pathCreator.path.length - 1) ? point + 1 : point;
        transform.position = pathCreator.path.GetPoint(p);
        distanceTraveled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);//pathCreator.path.GetClosestTimeOnPath(transform.position) * pathCreator.path.length;
        var findPointIndexData = pathCreator.path.CalculateClosestPointOnPathData(transform.position);
        s_savePoint = findPointIndexData.previousIndex;
        Debug.Log("_useTeleportToHighCheckPoint - s_savePoint " + s_savePoint);
    }
    bool stop = true;
    void Update()
    {
        if (pathCreator != null && !stop)
        {
            if (_moveBack)
            {
                distanceTraveled -= speed * Time.deltaTime;
            }
            else
            {
                distanceTraveled += speed * Time.deltaTime;
            }

            transform.position = pathCreator.path.GetPointAtDistance(distanceTraveled, endOfPathInstruction);
            Quaternion r = pathCreator.path.GetRotationAtDistance(distanceTraveled, endOfPathInstruction);
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
        distanceTraveled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag(Defined.TAG_CHECKPOINT))
        {
            if (other.GetComponent<MapCheckPoint>().checkPointType == _curCheckPoint && avoidUserClick)
            {
                stop = true;
                var data = pathCreator.path.CalculateClosestPointOnPathData(transform.position);
                s_savePoint = data.previousIndex;
                SetAnimation(idle, true);
                StartCoroutine(DelayGoToGame(_curCheckPoint));
            }
        }
    }
    /**
    * Click Action
    */
    public void OnCheckPointSelected(object ob)
    {
        var t = (CheckPointType)ob;
        if (!stop || avoidUserClick
         || ((int)t < (int)_curCheckPoint) && !UserInfo.GetInstance().IsLastLevelUnlocked()
        )
        {
            //char is moving, do not thing

        }
        else
        {
            avoidUserClick = true;
            Debug.Log("OnCheckPointSelected" + t);
            Debug.Log("_checkPointType" + _curCheckPoint);
            if (t == _curCheckPoint)
            {

                GoToGame(t);
                StopAllCoroutines();
            }
            else
            {
                stop = false;
                void _Move()
                {
                    transform.localScale = _curScale * new Vector2(_moveBack ? -1 : 1, 1);
                    s_saveCheckPoint = _curCheckPoint = t;
                    SetAnimation(run, true);
                };
                if ((int)t > (int)_curCheckPoint)
                {
                    _moveBack = false;
                    _Move();

                }
                else if (UserInfo.GetInstance().IsLastLevelUnlocked())
                {
                    _moveBack = true;
                    _Move();
                }


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
    }

}
