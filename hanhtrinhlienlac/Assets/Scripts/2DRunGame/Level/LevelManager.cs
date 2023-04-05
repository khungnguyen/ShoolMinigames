using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _bullet;
    public List<LevelInfo> _levels;

    private Camera camera;
    private Bounds _cameraBounds;
    public void SetCameraBounds(Bounds b)
    {
        _cameraBounds = b;
    }
    public LevelInfo FindLevel(GameEnum.LevelType level)
    {
        return _levels.Find(e => e.getLevelType() == level);
    }
    public void SpawnBullet()
    {
        var b = Instantiate<GameObject>(_bullet, transform);
        float randomY = _cameraBounds.min.y + UnityEngine.Random.Range(0, _cameraBounds.size.y);
        float x = _cameraBounds.max.x;
        var bullet = b.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetLevelBounds(_cameraBounds);
            bullet.setPosition(new Vector2(x, randomY));
        }
    }
    private void Awake() {
        _levels.ForEach(e => e.SetActive(false));
    }


}