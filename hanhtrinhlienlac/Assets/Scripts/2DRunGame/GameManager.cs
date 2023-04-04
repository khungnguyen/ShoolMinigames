using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MiniGames;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelManager _levelManager;
    [SerializeField] CinemachineVirtualCamera _cinemachineCamera;
    [SerializeField] Camera _gameCamera;
    [SerializeField] CinemachineConfiner _cinemachineConfiner;
    [SerializeField] HeroController _playerController;

    [SerializeField] UIManager _gameUI;

    [SerializeField] AudioSource _sound;
    [SerializeField] List<AudioClip> __soundData;

    public static GameManager inst;

    private Bounds _cameraBounds;



    [SerializeField]
    private GameEnum.LevelType _curLevel;
    void Awake()
    {
        inst = this;
    }
    void Start()
    {
        _playerController.OnLevelFinish += OnPlayerFinishMap;
        _playerController.OnPlayerDeath += OnPlayerDeath;
        _playerController.OnPlayerRevive += OnPlayerRevive;
        _playerController.OnCollect += OnPlayerCollect;
        _gameUI.getTutorManager().OnTutStart += OnTutorStart;
        _gameUI.getTutorManager().OnTutComplete += OnTutorEnd;
        LevelInfo startLevel = _levelManager.FindLevel(_curLevel);
        ChangeLevel(startLevel,true);
    }
    private void OnPlayerFinishMap(GameEnum.LevelType nextLevel)
    {
        LevelInfo next = _levelManager.FindLevel(nextLevel);
        if (next != null)
        {
            ChangeLevel(next);
            _curLevel = nextLevel;
        }
    }
    private void ChangeLevel(LevelInfo next, bool isEnter = false)
    {
        _gameUI.PauseScoring();
        PlayBGM(_curLevel);
        _gameUI.PlayTransitionEffect(isEnter,() =>
        {
            _cinemachineConfiner.m_BoundingShape2D = next.GetCinemachinConfinerData();
            var spawnPoint = next.GetStartPoint().getPosition();
            _playerController.SetParentLayer(next.GetPlayerLayer());
            _playerController.SetPosition(spawnPoint);
            _playerController.SetRestrictedArea(next.GetLevelBounds());
            _playerController.reset();
            _playerController.EnableInput(false);
            _playerController.GodMode(true);
            _gameUI.getTutorManager().SetTutType(GetTutByLevel(_curLevel)).ShowTutor(true);
            

        });
    }
    private void OnPlayerDeath(MarkedPoint revivePoint)
    {

    }
    private void OnPlayerRevive(MarkedPoint revivePoint)
    {
        if (revivePoint == null)
        {
            LevelInfo curLevel = _levelManager.FindLevel(_curLevel);
            var allRevivePoints = curLevel.GetAllRevivePoints();
            var result = allRevivePoints[0];
            foreach (MarkedPoint p in allRevivePoints)
            {
                if (p.getPosition().x > _playerController.GetPosition().x)
                {
                    break;
                }
                else
                {
                    result = p;
                }
            }
            _playerController.Revive(result);
        }
        else
        {
            _playerController.Revive(revivePoint);
        }


    }
    private void OnPlayerCollect(Coin g)
    {
        _gameUI.SetScore(Defined.BOUNS_SCORE);
        var wp = _gameUI.GetCoinHubWorldPos();
        g.SetTarget(wp,_gameCamera);
    }
    private void SpawnBullet()
    {
        _levelManager.SpawnBullet();
        Invoke("SpawnBullet", Random.Range(0.5f, 3f));
    }
    private void Update()
    {
        float height = 2f * _gameCamera.orthographicSize;
        float width = height * _gameCamera.aspect;
        var center = new Vector3(_gameCamera.transform.position.x, _gameCamera.transform.position.y);
        var size = new Vector3(width + 5, height);
        _cameraBounds = new Bounds(center, size);
        _levelManager.SetCameraBounds(_cameraBounds);
    }
    private void OnTutorStart(TutoriaType t)
    {

    }
    private void OnTutorEnd(TutoriaType t)
    {
        _playerController.EnableInput(true);
        _playerController.GodMode(false);
        _gameUI.StartScoring();
        //  SpawnBullet();
    }
    private TutoriaType GetTutByLevel(GameEnum.LevelType l)
    {
        return l switch
        {
            GameEnum.LevelType.Level1 => TutoriaType.Run2D_Game_1,
            GameEnum.LevelType.Level2 => TutoriaType.Run2D_Game_2,
            GameEnum.LevelType.Level3 => TutoriaType.Run2D_Game_3,
            GameEnum.LevelType.Level4 => TutoriaType.Run2D_Game_4,
            _ => TutoriaType.Run2D_Game_1,
        };
    }
    public void PlayBGM(GameEnum.LevelType index) {
        _sound.Stop();
        _sound.clip = __soundData[(int)index];
        _sound.Play();
        _sound.loop = true;
    }
}
