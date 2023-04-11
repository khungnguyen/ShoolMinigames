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
    [SpineAnimation]
    public string buffaloIdle;
    
    [SerializeField] float _minTimeIdle = 0.5f;
    [SerializeField] float _maxTimeIdle = 1.5f;
    [SerializeField] float _minDistanceMoveAround = 2f;
    [SerializeField] float _maxDistanceMoveAround = 5f;
    [SerializeField] ParticleSystem _moveParticle;
    private bool _buffaloMove = false;
    private bool __buffaloMoveAround = true;


    private Vector2 _initialPos;

    private bool _rideOx = false;
    private string _previousAnim = "";


    private void Start()
    {
        _initialPos = transform.position;
        ChangeSkin("default");
        StartCoroutine(MoveAround());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Defined.TAG_PLAYER) && !_rideOx)
        {
            _player = other.transform.GetComponent<HeroController>();
            if (!_player.Grounded && _player.GetPosition().y >= transform.position.y + 1f)
            {
                _player.SetPosition(attachSeatPoint.position);
                _buffaloMove = true;
                __buffaloMoveAround = false;
                SoundManager.inst.PlaySfx(__soundData[1], true);
                _player.EnableInput(false);
                setAnimation(buffaloWalk, true);
                _player.RideTheOx(true);

                ChangeSkin(UserInfo.GetInstance().GetSkin());
                _rideOx = true;
                if(transform.localScale.x == -1) {
                    transform.localScale = new Vector2(1,1);
                }
                
            }
        }
        else if (other.CompareTag(Defined.TAG_ENDPOINT))
        {
            Debug.LogError("Buffalo stop");
            _player.EnableInput(true);
            _player.RideTheOx(false);
            _buffaloMove = false;
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
            SoundManager.inst.StopSfx();

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
                SoundManager.inst.PlaySfx(__soundData[0]);
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag(Defined.TAG_COLLECTABLE))
        {
            if(other.TryGetComponent<Coin>(out  var coin)){
                coin.setScore(Defined.BONUS_SCORE_BUFFALO);
                _player.OnCoinCollect(coin);
             }
     
        }
    }
    private int _direction = 0;
    IEnumerator MoveAround()
    {
        float  distance = UnityEngine.Random.Range(_minDistanceMoveAround, _maxDistanceMoveAround);
        setAnimation(buffaloWalk, true);
        _direction = 1;
        transform.localScale = new Vector2(_direction, 1);
        float idleTime = -1;
        int previousDir = _direction;
        while (true)
        {
            if (_buffaloMove)
            {
                break;
            }
            else if (idleTime > 0)
            {
                idleTime -= Time.deltaTime;
                yield return null;
            }
            else if ((transform.position.x > _initialPos.x + distance) && _direction ==1 || (transform.position.x < _initialPos.x - distance)&&_direction ==-1)
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
                JumpDown = false,
                JumpUp = false,
                X = _buffaloMove ? 1f : 0f
            };
        }
    }
    void Update()
    {
        if (__buffaloMoveAround)
        {
            // var x =
        }
        else if (_player != null && _buffaloMove)
        {

            _player.SetPosition(attachSeatPoint.position);

        }
        base.Update();

    }

    private void setAnimation(String name, bool loop)
    {
        if (_previousAnim.Equals(name)) return;
        _previousAnim = name;
        _skeleton.AnimationState.SetAnimation(0, name, loop);
        if(name.Equals(buffaloIdle)) {
            _moveParticle.Stop();
        }
        else if(name.Equals(buffaloWalk)) {
            _moveParticle.Play();
        }
    
    }
    private void stopAniamtion(int track)
    {
        _skeleton.AnimationState.ClearTrack(track);
    }
}
