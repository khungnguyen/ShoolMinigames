using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniGames;
using Unity.VisualScripting;
using Spine.Unity;
using System;
using System.Buffers;
using Mono.Cecil.Cil;

public class BuffaloController : BaseController
{
    public SkeletonAnimation _skeleton;
    private HeroController _player;

    public Transform attachSeatPoint;

    public ParticleSystem dustVfx;
    [SerializeField] AudioSource _sound;
    [SerializeField] List<AudioClip> __soundData;


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
            if (!_player.Grounded)
            {
                _player.SetPosition(attachSeatPoint.position);
                _buffaloMove = true;
                _sound.clip = __soundData[1];
                _sound.loop = true;
                _sound.Play();
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
            _sound.Stop();

        }
        else if (other.CompareTag("Obstacle"))
        {
            //TODO
            {
                Bounds b = (other.transform.GetComponent<Collider2D>()).bounds;
                float w = b.max.x - b.min.x;
                float h = b.max.y - b.min.y;
                var newPos = new Vector2(other.transform.position.x + w, other.transform.position.y - h / 2);
                var par = Instantiate(dustVfx, newPos, other.transform.rotation);
                Destroy(par.gameObject, 2f);
                _sound.PlayOneShot(__soundData[0]);
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
