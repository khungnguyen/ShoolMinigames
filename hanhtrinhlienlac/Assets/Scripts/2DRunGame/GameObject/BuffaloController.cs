using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniGames;
using Unity.VisualScripting;
using Spine.Unity;
using System;
using System.Buffers;

public class BuffaloController : PlayerController
{
    public SkeletonAnimation _skeleton;
    private HeroController _player;

    public Transform attachSeatPoint;

    public ParticleSystem dustVfx;

    [SpineAnimation]
    public string buffaloWalk;

    private bool _buffaloMove = false;

    private Vector2 _initialPos;
    private void Start()
    {
        _initialPos = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.transform.GetComponent<HeroController>();
            Debug.LogError("OnTriggerEnter2D Player");
            if (!_player.Grounded)
            {
                _player.SetPosition(attachSeatPoint.position);
                _buffaloMove = true;
                _player.EnableInput(false);
                setAnimation(buffaloWalk, true);
                _player.RideTheOx(true);
            }
        }
        else if (other.CompareTag("EndPoint"))
        {
            Debug.LogError("Buffalo stop");
            _player.EnableInput(true);
            _player.RideTheOx(false);
            _player.Activate();
            _buffaloMove = false;
            stopAniamtion(0);
            death();
            //  transform.position = _initialPos;
            Input = new FrameInput
            {
                JumpDown = false,
                JumpUp = false,
                X = 0f
            };

        }
        else if (other.CompareTag("Obstacle"))
        {
            //TODO
            {
                var par = Instantiate(dustVfx, other.transform.position, other.transform.rotation);
                Destroy(par, 2f);
            }
            Destroy(other.gameObject);
        }
    }
    public override void GatherInput()
    {

        Input = new FrameInput
        {
            JumpDown = false,
            JumpUp = false,
            X = _buffaloMove ? 1f : 0f
        };
    }
    void Update()
    {
        if (_player != null && _buffaloMove)
        {
            _player.SetPosition(attachSeatPoint.position);
        }
        base.Update();

    }
    private void setAnimation(String name, bool loop)
    {
        _skeleton.AnimationState.SetAnimation(0, name, loop);
    }
    private void stopAniamtion(int track)
    {
        _skeleton.AnimationState.ClearTrack(track);
    }
}
