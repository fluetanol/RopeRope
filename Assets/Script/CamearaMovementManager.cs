using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamearaMovementManager : MonoBehaviour
{
    public Transform Target;
    public float _cameraMoveSpeed;
    public float _yPositionControl;
    public SpriteRenderer _backGround;
    private Vector2 _initialiSize;
    // Start is called before the first frame update

    void Start() => _initialiSize = _backGround.size;

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition =  this.transform.position;
        Vector3 NextPosition = PlayerStatus.NextPos;
        NextPosition.y = NextPosition.y + _yPositionControl;
        currentPosition.x = Target.position.x;
        currentPosition.y = Target.position.y;
        Vector3 lerpPosition = Vector2.Lerp(transform.position, NextPosition,
                                Time.deltaTime * _cameraMoveSpeed);

        lerpPosition.z = -10;
        transform.position = lerpPosition;

        lerpPosition.z = 0;
        lerpPosition.y = _backGround.transform.position.y;
        _backGround.transform.position = lerpPosition;
        
        //1m당 1.65배 증가
        //


        Vector2 diff = Vector2.zero;
        diff.x = PlayerStatus.NextPos.x - PlayerStatus.CurrentPos.x;
        _backGround.size = _backGround.size + diff * Time.deltaTime;
        if(_backGround.size.x < _initialiSize.x) {
            _backGround.size = _initialiSize;
        }
    }
}
