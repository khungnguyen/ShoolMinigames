using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniGames;
using Unity.VisualScripting;

public class BuffaloController : PlayerController
{
    private PlayerController _player;
    private bool _buffaloMove = false;

    private Vector2 _initialPos;
    private void Start() {
        _initialPos = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.transform.GetComponent<PlayerController>();
            Debug.LogError("OnTriggerEnter2D" + _player.Grounded);
            if(!_player.Grounded) {
                 _player.SetPosition(transform.position);
                 _buffaloMove = true;
                 _player.EnableInput(false);
            }
        }
        else if (other.CompareTag("EndPoint"))
        {
            _player.EnableInput(true);
            _buffaloMove = false;
            transform.position = _initialPos;
            Input = new FrameInput
            {
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = 0f
            };
        }
    }
    void Update()
    {
        base.Update();
        if (_player != null && _buffaloMove)
        {
            _player.SetPosition(transform.position);
            Input = new FrameInput
            {
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = 1f
            };
        }
    }
}
