using System;
using System.Collections;
using System.Collections.Generic;
using MiniGames;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HeroController : BaseController
{

    public bool rideTheOx = false;
    public bool godMode = false;
    public bool finishLevel = false;
    [SerializeField] GameObject _dustVfx;


    private MarkedPoint _revivePoint;
    public System.Action<GameEnum.LevelType> OnLevelFinish;
    public System.Action<MarkedPoint> OnPlayerDeath;
    public System.Action<MarkedPoint> OnPlayerRevive;
    public System.Action<int> OnCollect;

    private int _curScore = 0;

    public void RideTheOx(bool enable)
    {
        rideTheOx = enable;
        Deactivate();
        GodMode(enable);
    }

    public override void death()
    {
        EnableInput(false);
        base.death();
        OnPlayerDeath?.Invoke(_revivePoint);
    }

    public void GodMode(bool enable)
    {
        godMode = enable;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (godMode || isDie || finishLevel) return;
        Debug.LogError("OnTriggerEnter2D" + other.tag);
        if (other.CompareTag(Defined.TAG_ENDPOINT))
        {
            if (OnLevelFinish != null)
            {
                var next = other.GetComponent<FinishLevelPoint>().nextLevel;
                finishLevel = true;
                EnableInput(false);
                StartCoroutine(TriggerFinshLevelEvent(next, 2));
            }

        }
        else if (other.CompareTag(Defined.TAG_OBSTACLE))
        {
            _revivePoint = other.GetComponent<Obstacle>()?.revivePoint;
            death();

        }
        else if (other.CompareTag(Defined.TAG_COLLECTABLE))
        {
            _curScore++;
            OnCollect?.Invoke(_curScore);
            Destroy(other.gameObject);

        }
    }
    public void reset()
    {
        finishLevel = false;
        isDie = false;
        godMode = false;
        EnableInput(true);
    }
    IEnumerator TriggerFinshLevelEvent(GameEnum.LevelType next, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnLevelFinish.Invoke(next);
    }
    public void SetOnPlayerFinishMap(Action<GameEnum.LevelType> callback)
    {
        OnLevelFinish = callback;
    }
    public void EnableInput(bool e)
    {
        enableInput = e;
        if (!e)
        {
            Input = new FrameInput
            {
                JumpDown = false,
                JumpUp = false,
                X = 0f
            };
        }
    }
    public override void GatherInput()
    {
        if (enableInput)
        {
            Input = new FrameInput
            {
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = UnityEngine.Input.GetAxisRaw("Horizontal")
            };
            if (Input.JumpDown)
            {
                lastJumpPressed = Time.time;
            }
        }

    }
    void Update()
    {
        base.Update();
    }
    public SkeletonAnimation spineColor;
    public void Revive(MarkedPoint point)
    {
        if (point != null)
        {
            HideSpine();
            SetPosition(point.getPosition());
            isDie = false;
            EnableInput(true);
            point = null;
            var dust = Instantiate<GameObject>(_dustVfx, new Vector2(transform.position.x, transform.position.y - GetCharBounds().size.y / 2), Quaternion.identity);
            Destroy(dust, 1f);
            Invoke("ShowSpine", 0.4f);

        }
    }
    public void NotifyRevive()
    {
        OnPlayerRevive.Invoke(_revivePoint);
    }
    private void HideSpine()
    {
        TransparentSpine(true);
    }
    private void ShowSpine()
    {
        TransparentSpine(false);
    }
    private void TransparentSpine(bool e = true)
    {
        Spine.Skeleton skeleton = spineColor.skeleton;
        skeleton.A = e ? 0f : 1f;
    }
}
