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
    [SerializeField] AudioSource _sound;
    [SerializeField] List<AudioClip> __soundData;


    private MarkedPoint _revivePoint;
    public System.Action<GameEnum.LevelType> OnLevelFinish;
    public System.Action<MarkedPoint> OnPlayerDeath;
    public System.Action<MarkedPoint> OnPlayerRevive;
    public System.Action<Coin> OnCollect;

    private int _curScore = 0;

    private bool _autoMove;
    public bool inWater = false;

    public enum SOUND
    {
        COIN,
        DIE,
        VICTORY
    }

    public void RideTheOx(bool enable)
    {
        rideTheOx = enable;
        GodMode(enable);
        TransparentSpine(enable);
        if (enable)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public override void death()
    {
        EnableInput(false);
        base.death();
        OnPlayerDeath?.Invoke(_revivePoint);
        PlaySFX(SOUND.DIE);
    }

    public void GodMode(bool enable)
    {
        godMode = enable;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDie || finishLevel) return;
        Debug.LogError("OnTriggerEnter2D" + other.tag);
        if (other.CompareTag(Defined.TAG_ENDPOINT))
        {
            if (OnLevelFinish != null)
            {
                var next = other.GetComponent<FinishLevelPoint>().nextLevel;
                finishLevel = true;
                EnableInput(false);
                PlaySFX(SOUND.VICTORY);

                if (next == GameEnum.LevelType.EndGame)
                {

                    StartCoroutine(AutoMoveToSoilder(2));
                }
                else
                {
                    StartCoroutine(TriggerFinshLevelEvent(next, 2));
                }
            }

        }
        else if (other.CompareTag(Defined.TAG_OBSTACLE) && !godMode)
        {
            _revivePoint = other.GetComponent<Obstacle>()?.revivePoint;
            if (other.GetComponent<Obstacle>().objectType == GameEnum.ObstacleType.WATER)
            {
                inWater = true;


            }
            else
            {
                inWater = false;
            }
            death();
        }
        else if (other.CompareTag(Defined.TAG_COLLECTABLE))
        {

            OnCoinCollect(other.GetComponent<Coin>());
        }
        else if (other.CompareTag(Defined.TAG_SOILDER))
        {
            _autoMove = false;
            //  Deactivate();
            StartCoroutine(TriggerFinshLevelEvent(GameEnum.LevelType.EndGame, 2));
        }
    }
    public void OnCoinCollect(Coin c)
    {
        _curScore++;
        OnCollect?.Invoke(c);
        PlaySFX(SOUND.COIN);
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
    }
    public override void GatherInput()
    {
        if (enableInput)
        {
            Input = new FrameInput
            {
                JumpDown = _autoMove ? false : UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = _autoMove ? false : UnityEngine.Input.GetButtonUp("Jump"),
                X = _autoMove ? 1 : UnityEngine.Input.GetAxisRaw("Horizontal")
            };
            if (Input.JumpDown)
            {
                lastJumpPressed = Time.time;
            }
        }
        else
        {
            Input = new FrameInput
            {
                JumpDown = false,
                JumpUp = false,
                X = 0f
            };
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
        Spine.Skeleton skeleton = spine.skeleton;
        skeleton.A = e ? 0f : 1f;
    }
    public void SetParentLayer(Transform parent)
    {
        transform.parent = parent;
    }
    public void PlaySFX(SOUND index)
    {
        SoundManager.inst.PlaySfx(__soundData[(int)index]);
    }
    IEnumerator AutoMoveToSoilder(int time)
    {
        yield return new WaitForSeconds(time);
        EnableInput(true);
        _autoMove = true;
        finishLevel = false;
    }

}
