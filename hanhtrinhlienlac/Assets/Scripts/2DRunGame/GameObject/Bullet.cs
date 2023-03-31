using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : Obstacle
{
    // Start is called before the first frame update
    [SerializeField] float _speed = 10f;
    private Bounds _bound;

    public void SetLevelBounds(Bounds _b)
    {
        _bound = _b;
    }
    void Update()
    {
        Vector2 velocity = Vector2.right * _speed * Time.deltaTime;
        transform.position = new Vector2(transform.position.x, transform.position.y) - velocity;
        if (transform.position.x < _bound.min.x) {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(Defined.TAG_PLAYER)) {
             Destroy(gameObject);
        }
    }
}
