using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamearaMovementManager : MonoBehaviour
{
    public Transform Target;
    public float _cameraMoveSpeed;
    public float _yPositionControl;

    public SpriteRenderer _backGround;
    public List<SpriteRenderer> _backGrounds;

    private Vector2 _initialiSize;

    // Start is called before the first frame update

    void Start() => _initialiSize = _backGround.size;

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition =  this.transform.position;
        Vector3 nextPosition = PlayerStatus.NextPos;
        Vector3 lerpPosition = SetCameraNextPosition(currentPosition, nextPosition);
        SetBackGroundMove(lerpPosition);
    }


    private Vector3 SetCameraNextPosition(Vector3 currentPosition, Vector3 nextPosition){
        nextPosition.y = nextPosition.y + _yPositionControl;
        currentPosition.x = Target.position.x;
        currentPosition.y = Target.position.y;
        Vector3 lerpPosition = Vector2.Lerp(transform.position, nextPosition, Time.deltaTime * _cameraMoveSpeed);

        lerpPosition.z = -10;
        if(PlayerStateManager.Instance.PlayerInputState != EPlayerInputState.Launch)
        transform.position = lerpPosition;
        return lerpPosition;
    }

    private void SetBackGroundMove(Vector3 lerpPosition){
        lerpPosition.z = 0;
        lerpPosition.y = _backGround.transform.position.y;
        _backGround.transform.position = lerpPosition;

        //1m당 1.65배 증가

        Vector2 diff = Vector2.zero;
        diff.x = PlayerStatus.NextPos.x - PlayerStatus.CurrentPos.x;
        _backGround.size = _backGround.size + diff * Time.deltaTime;
        if (_backGround.size.x < _initialiSize.x){
            _backGround.size = _initialiSize;
        }
    }

}
