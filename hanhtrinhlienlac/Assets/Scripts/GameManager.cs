using System.Collections;
using System.Collections.Generic;
using MiniGames;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelManager _levelManager;
    [SerializeField] Transform _charLayer;
    [SerializeField] GameObject _charPrefab;
    [SerializeField] Camera _camera;



    private PlayerController _charController;

    private FollowCamera _followCamera;

    public static GameManager inst;



    void Awake() {
        inst = this;
        _followCamera = _camera.GetComponent<FollowCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {
    //    _levelManager.LoadMap(GameEnum.MapLevel.Level1);
    //    _levelManager.SetCameraForParallax(_camera.transform);
    //    GameObject ob = Instantiate(_charPrefab,_charLayer);
    //    _charController = ob.GetComponent<PlayerController>();
    //    _charController.SetPosition(_levelManager.GetStartPoint().getPosition());
    //    _followCamera.SetTarget(_charController.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
