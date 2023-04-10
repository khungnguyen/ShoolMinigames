using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : ObjectBase
{
    // Start is called before the first frame update
    private Vector2 _target;
    private Camera _cam;
    private bool _fly = false;

    private int _score = 1;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_fly)
        {
            var wp = _cam.ScreenToWorldPoint(_target);
            transform.position = Vector2.Lerp(transform.position, wp, 6f * Time.deltaTime);
            var distance = Vector2.Distance(transform.position, wp);
            if (distance < 1f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetTarget(Vector2 p,Camera c)
    {
        _target = p;
        _cam = c;
        _fly = true;
    }
    public void setScore(int i) {
        _score = i;
    }
    public int GetScore() {
        return _score;
    }

}
