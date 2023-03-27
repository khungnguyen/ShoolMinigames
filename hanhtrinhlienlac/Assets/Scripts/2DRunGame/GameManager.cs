using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MiniGames;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelManager _levelManager;
    [SerializeField] CinemachineVirtualCamera _cinemachineCamera;
    [SerializeField] CinemachineConfiner _cinemachineConfiner;
    [SerializeField] HeroController _playerController;

    public static GameManager inst;



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
        LevelInfo startLevel = _levelManager.FindLevel(_curLevel);
        ChangeLevel(startLevel);
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
    private void ChangeLevel(LevelInfo next)
    {
        _cinemachineConfiner.m_BoundingShape2D = next.GetCinemachinConfinerData();
        var spawnPoint = next.GetStartPoint().getPosition();
        _playerController.SetPosition(spawnPoint);
        _playerController.SetRestrictedArea(next.GetLevelBounds());
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
    // Start is called before the first frame update

}
