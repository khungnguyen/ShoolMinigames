using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MiniGames;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

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
    private GameEnum.LevelType _previous;
    void Awake()
    {
        inst = this;
    }
    void Start()
    {
        _playerController.ChangeSkin(UserInfo.GetInstance().GetSkin());
        _playerController.OnLevelFinish += OnPlayerFinishMap;
        _playerController.OnPlayerDeath += OnPlayerDeath;
        _playerController.OnPlayerRevive += OnPlayerRevive;
        _playerController.OnCollect += OnPlayerCollect;
        _gameUI.getTutorManager().OnTutStart += OnTutorStart;
        _gameUI.getTutorManager().OnTutComplete += OnTutorEnd;
        LevelInfo startLevel = _levelManager.FindLevel(_curLevel);
        ChangeLevel(startLevel, true);

    }
    private void OnPlayerFinishMap(GameEnum.LevelType nextLevel)
    {
        LevelInfo next = _levelManager.FindLevel(nextLevel);
        if (next != null)
        {
            _previous = _curLevel;
            _curLevel = nextLevel;
            ChangeLevel(next);
        }
        else
        {
            OnEndGame();
        }
    }
    private void ChangeLevel(LevelInfo next, bool isEnter = false)
    {

        _gameUI.PauseScoring();
        PlayBGM(_curLevel);
        _gameUI.PlayTransitionEffect(isEnter, () =>
        {
            if (_curLevel != _previous)
            {
                LevelInfo previous = _levelManager.FindLevel(_previous);
                previous?.SetActive(false);
                Destroy(previous.gameObject);
            }
            next.SetActive(true);
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
        _gameUI.SetScore(g.GetScore());
        var wp = _gameUI.GetCoinHubWorldPos();
        g.SetTarget(wp, _gameCamera);
    }
    private void SpawnBullet()
    {
        _levelManager.SpawnBullet();
        Invoke("SpawnBullet", Random.Range(2f, 5f));
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
        _gameUI.PauseScoring();
    }
    private void OnTutorEnd(TutoriaType t)
    {
        if (t == TutoriaType.Run2D_Game_End)
        {
            var score = _gameUI.GetScoring().CurRemainingTimeScore;
            _gameUI.GetRewardUI().Show(score.ToString(), () =>
            {
                _gameUI.GetScoring().Submit("game3");
                BackToMainMenu();
            });
        }
        else
        {
            _playerController.EnableInput(true);
            _playerController.GodMode(false);
            _gameUI.StartScoring();
            if (t == TutoriaType.Run2D_Game_4)
            {
                SpawnBullet();
            }
            else
            {
                CancelInvoke("SpawnBullet");
            }
        }
    }
    private TutoriaType GetTutByLevel(GameEnum.LevelType l)
    {
        return l switch
        {
            GameEnum.LevelType.Level1 => TutoriaType.Run2D_Game_1,
            GameEnum.LevelType.Level2 => TutoriaType.Run2D_Game_2,
            GameEnum.LevelType.Level3 => TutoriaType.Run2D_Game_3,
            GameEnum.LevelType.Level4 => TutoriaType.Run2D_Game_4,
            GameEnum.LevelType.EndGame => TutoriaType.Run2D_Game_End,
            _ => TutoriaType.Run2D_Game_1,
        };
    }
    public void PlayBGM(GameEnum.LevelType index)
    {
        SoundManager.inst.StopBGM();
        SoundManager.inst.PlayBGM(__soundData[(int)index], true);
    }
    private void OnEndGame()
    {
        UserInfo.GetInstance().SetCompletedRunGame(true);
        UserInfo.GetInstance().SetUnlockLevel(CheckPointType.CHECK_POINT_4, true);
        _gameUI.getTutorManager().SetTutType(TutoriaType.Run2D_Game_End).ShowTutor(true);
    }
    private void BackToMainMenu()
    {
        SceneManager.LoadScene("Main");
    }
}
