using System;
using System.Collections;
using System.Collections.Generic;
using MiniGames;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HeroController : PlayerController
{

    public bool rideTheOx = false;
    public bool godMode = false;

    private MarkedPoint _revivePoint;
    public System.Action<GameEnum.LevelType> OnLevelFinish;
    public System.Action<MarkedPoint> OnPlayerDeath;
    public System.Action<MarkedPoint> OnPlayerRevive;

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
        if (godMode) return;
        // Debug.LogError("OnTriggerEnter2D" + other.tag);
        if (other.CompareTag("EndPoint"))
        {
            if (OnLevelFinish != null)
            {
                var next = other.GetComponent<MarkedPoint>().nextLevel;
                OnLevelFinish.Invoke(next);
            }

        }
        else if (other.CompareTag("Obstacle"))
        {
            _revivePoint = other.GetComponent<Obstacle>()?.revivePoint;
            // if (true)
            // {
            //     EnableBounce();
            // }
            // else
            {
              death();
            }


        }
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
    public void Revive(MarkedPoint point)
    {
        if (point != null)
        {
            SetPosition(point.getPosition());
            isDie = false;
            EnableInput(true);
            point = null;
        }
    }
    public void NotifyRevive()
    {
        OnPlayerRevive.Invoke(_revivePoint);
    }
}
