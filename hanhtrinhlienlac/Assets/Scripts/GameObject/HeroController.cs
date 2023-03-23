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

    private System.Action<GameEnum.LevelType> _onLevelFinish;

    public void RideTheOx(bool enable)
    {
        rideTheOx = enable;
        GodMode(enable);
    }

    public override void death()
    {
        EnableInput(false);
        base.death();
    }
    public void GodMode(bool enable) {
        godMode = enable;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(godMode) return;
       // Debug.LogError("OnTriggerEnter2D" + other.tag);
        if (other.CompareTag("EndPoint"))
        {
            if (_onLevelFinish != null)
            {
                var next = other.GetComponent<MarkedPoint>().nextLevel;
                _onLevelFinish.Invoke(next);
            }

        }
        else if(other.CompareTag("Obstacle")) {
            death();
        }
    }
    public void SetOnPlayerFinishMap(Action<GameEnum.LevelType> callback)
    {
        _onLevelFinish = callback;
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
    void Update() {
        base.Update();
    }
}
