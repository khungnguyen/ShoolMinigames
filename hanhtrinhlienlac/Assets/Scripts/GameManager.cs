using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MiniGames;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelManager _levelManager;
    [SerializeField] CinemachineVirtualCamera _cinemachineCamera;
    [SerializeField] CinemachineConfiner _cinemachineConfiner;
    [SerializeField] PlayerController _playerController;

    public static GameManager inst;



    void Awake() {
        inst = this;
    }
    private void Start() {
        _playerController.SetOnPlayerFinishMap(OnPlayerFinishMap);
    }
    private void OnPlayerFinishMap(GameEnum.LevelType nextLevel) {
        LevelInfo next = _levelManager.FindLevel(nextLevel);
        if(next != null) {
            var spawnPoint = next.GetStartPoint().getPosition();
            _playerController.SetPosition(spawnPoint);
            _cinemachineConfiner.m_BoundingShape2D = next.GetCinemachinConfinerData();
        }
    }
    // Start is called before the first frame update
   
}
