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

    public Transform attachSeatPoint;

    public ParticleSystem dustVfx;
    [SerializeField] AudioSource _sound;
    [SerializeField] List<AudioClip> __soundData;


    [SpineAnimation]
    public string buffaloWalk;
    [SpineAnimation]
    public string buffaloIdle;
     [SpineAnimation]
    public string buffaloJump;

    [SerializeField] float _minTimeIdle = 0.5f;
    [SerializeField] float _maxTimeIdle = 1.5f;
    [SerializeField] float _minDistanceMoveAround = 2f;
    [SerializeField] float _maxDistanceMoveAround = 5f;
    [SerializeField] ParticleSystem _moveParticle;
    [Range(0, 8)]
    [SerializeField] float _distanceDetectHuman = 2f;

    [SerializeField] private HeroController _player;
    private bool _buffaloMoveStraight = false;
    private bool __buffaloMoveAround = true;


    private Vector2 _initialPos;

    private bool _rideOx = false;
    private string _previousAnim = "";

    private bool _isPlayerNearOx = false;


    private void Start()
    {
        _initialPos = transform.position;
        ChangeSkin("default");
        StartCoroutine(MoveAround());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Defined.TAG_PLAYER))
        {
            _player = other.transform.GetComponent<HeroController>();
            if (!_player.Grounded
            && !_rideOx
            && _player.GetPosition().y >= transform.position.y + GetCharBounds().size.y
            && _player.GetPosition().x <= transform.position.x
            && _player.GetPosition().x > transform.position.x - GetCharBounds().size.x / 2
            )
            {
                _player.SetPosition(attachSeatPoint.position);
                _buffaloMoveStraight = true;
                __buffaloMoveAround = false;
                 PlaySFX(__soundData[1], true);
                _player.EnableInput(false);
                setAnimation(buffaloWalk, true);
                _player.RideTheOx(true);
                ChangeSkin(UserInfo.GetInstance().GetSkin());
                _rideOx = true;
                if (transform.localScale.x == -1)
                {
                    transform.localScale = new Vector2(1, 1);
                }

            }
            else
            {

            }
        }
        else if (other.CompareTag(Defined.TAG_ENDPOINT))
        {
            Debug.LogError("Buffalo stop");
            _player.EnableInput(true);
            _player.RideTheOx(false);
            _buffaloMoveStraight = false;
            setAnimation(buffaloIdle, true);
            ChangeSkin("default");
            death();
            //  transform.position = _initialPos;
            Input = new FrameInput
            {
                JumpDown = false,
                JumpUp = false,
                X = 0f
            };
            soundManager.StopSfx();

        }
        else if (other.CompareTag(Defined.TAG_OBSTACLE))
        {
            //TODO
            {
                Bounds b = (other.transform.GetComponent<Collider2D>()).bounds;
                float w = b.max.x - b.min.x;
                float h = b.max.y - b.min.y;
                var newPos = new Vector2(other.transform.position.x + w, other.transform.position.y - h / 2);
                var par = Instantiate(dustVfx, newPos, other.transform.rotation);
                Destroy(par.gameObject, 2f);
                PlaySFX(__soundData[0]);
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag(Defined.TAG_COLLECTABLE))
        {
            if (other.TryGetComponent<Coin>(out var coin))
            {
                coin.setScore(Defined.BONUS_SCORE_BUFFALO);
                _player.OnCoinCollect(coin);
            }

        }
    }
    private int _direction = 0;
    IEnumerator MoveAround()
    {
        float distance = UnityEngine.Random.Range(_minDistanceMoveAround, _maxDistanceMoveAround);
        setAnimation(buffaloWalk, true);
        _direction = 1;
        transform.localScale = new Vector2(_direction, 1);
        float idleTime = -1;
        int previousDir = _direction;
        while (true)
        {
            if (_buffaloMoveStraight)
            {
                break;
            }
            else if (idleTime > 0)
            {
                idleTime -= Time.deltaTime;
                yield return null;
            }
            else if ((transform.position.x > _initialPos.x + distance) && _direction == 1 || (transform.position.x < _initialPos.x - distance) && _direction == -1)
            {
                idleTime = UnityEngine.Random.Range(_minTimeIdle, _maxTimeIdle);
                previousDir = _direction;
                _direction = 0;
                setAnimation(buffaloIdle, true);
                distance = UnityEngine.Random.Range(_minDistanceMoveAround, _maxDistanceMoveAround);
            }
            else
            {
                setAnimation(buffaloWalk, true);
                _direction = -previousDir;
                transform.localScale = new Vector2(_direction, 1);
            }
            if (idleTime > 0 && _player && Vector2.Distance(_player.GetPosition(), GetPosition()) < 2f)
            {
                idleTime = 0;
                Debug.Log("ALERT! HUMAN IS NEAR BY");
                //forcing move
            }
            yield return null;
        }
    }
    public override void GatherInput()
    {
        if (__buffaloMoveAround)
        {
            Input = new FrameInput
            {
                JumpDown = false,
                JumpUp = false,
                X = _direction,
            };
        }
        else
        {
            Input = new FrameInput
            {
                JumpDown = _buffaloMoveStraight ? UnityEngine.Input.GetButtonDown("Jump") : false,
                JumpUp = _buffaloMoveStraight ? UnityEngine.Input.GetButtonUp("Jump") : false,
                X = _buffaloMoveStraight ? 1f : 0f
            };
            if (Input.JumpDown)
            {
                lastJumpPressed = Time.time;
            }
        }
    }
    void Update()
    {
        if (__buffaloMoveAround)
        {
            // var x =
        }
        else if (_player != null && _buffaloMoveStraight)
        {

            _player.SetPosition(attachSeatPoint.position);

        }
        base.Update();
         if (JumpingThisFrame) {
            setAnimation(buffaloJump,false);
         }
         if (Grounded && !JumpingThisFrame && !LandingThisFrame)
            {
                if (Input.X != 0)
                {
                    setAnimation(buffaloWalk,true);
                }
                else
                {
                    setAnimation(buffaloIdle,true);
                }
            }

    }

    private void setAnimation(String name, bool loop)
    {
        if (_previousAnim.Equals(name)) return;
        _previousAnim = name;
        _skeleton.AnimationState.SetAnimation(0, name, loop);
        if (name.Equals(buffaloIdle))
        {
            _moveParticle.Stop();
        }
        else if (name.Equals(buffaloWalk))
        {
            _moveParticle.Play();
        }

    }
    private void stopAniamtion(int track)
    {
        _skeleton.AnimationState.ClearTrack(track);
    }
}
