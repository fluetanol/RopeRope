using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsCollideManager : SingletonMonobehavior<PlayerPhysicsCollideManager>
{
    private Collider2D _playerCollider2d;
    private RaycastHit2D[] _hits;

    new void Awake()
    {
        base.Awake();
        SetInstance();
    }

    void Start()
    {
        _playerCollider2d = GetComponent<CapsuleCollider2D>();
        _hits = new RaycastHit2D[10];
    }

    void FixedUpdate()
    {
        int count = _playerCollider2d.Cast(Vector2.zero, _hits);
        if (count > 0) {
            Vector2 point = _hits[0].point;
            point.x = transform.position.x;
            transform.position = point;
        };
        
    }

    protected override void SetInstance() => Instance = this;
}
