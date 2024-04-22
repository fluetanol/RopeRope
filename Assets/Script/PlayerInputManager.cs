

using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public interface INormalMove
{
    public void OnMove(InputAction.CallbackContext callback);
    public void OnJump(InputAction.CallbackContext callback);
}

//Player Input을 처리하는 곳
public class PlayerInputManager : SingletonMonobehavior<PlayerInputManager>, INormalMove
{
    private Rigidbody2D _playerRigidBody2d;
    private Collider2D _playerCollider2d;
    private RaycastHit2D[] _hits;
    private RaycastHit2D _landHit; 
    private Vector2 _moveVector;
    public int hitCount;
    public float KoyoteTime;

    float time;

    void Start(){
        _playerRigidBody2d = GetComponent<Rigidbody2D>();
        _playerCollider2d = GetComponent<CapsuleCollider2D>();
        _hits = new RaycastHit2D[10];
    }

    public void FixedUpdate(){
        Move();
        if(PlayerStatus.isKoyote && KoyoteTime >= PlayerStatus.KoyoteTime){
            PlayerStatus.isKoyote = false;
            KoyoteTime = 0;
        }
        else if(PlayerStatus.isKoyote){
            KoyoteTime += Time.fixedDeltaTime;
        }
    }

    public void OnJump(InputAction.CallbackContext callback){
        if(callback.performed && (PlayerStateManager.Instance.PlayerState != EPlayerState.Air || PlayerStatus.isKoyote)) 
            PlayerStateManager.Instance.PlayerState = EPlayerState.Jump;
    }

    public void OnMove(InputAction.CallbackContext callback){
        _moveVector = callback.ReadValue<Vector2>();
    }

    public void Move(){;
        //player run 상태 변경 요소
        PlayerStatus.CurrentPos = _playerRigidBody2d.position;
        
        if (PlayerStatus.CurrentMoveDirection.x > _moveVector.x){
            PlayerStatus.CurrentMoveDirection.x -= 0.1f;
            if(_moveVector.x == 0  && PlayerStatus.CurrentMoveDirection.x < 0) 
            PlayerStatus.CurrentMoveDirection.x = 0;
        }
        else if (PlayerStatus.CurrentMoveDirection.x < _moveVector.x){
            PlayerStatus.CurrentMoveDirection.x += 0.1f;
            if (_moveVector.x == 0 && PlayerStatus.CurrentMoveDirection.x > 0) 
            PlayerStatus.CurrentMoveDirection.x = 0;
        }

        if(PlayerStateManager.Instance.PlayerState == EPlayerState.Air){
            time+=Time.fixedDeltaTime;
            PlayerStatus.CurrentAirDirection = Vector2.down;
        }
        else if(PlayerStateManager.Instance.PlayerState == EPlayerState.Jump){
            time += Time.fixedDeltaTime;
            PlayerStatus.CurrentAirDirection = Vector2.down;
            PlayerStatus.CurrentMoveDirection.y = PlayerStatus.Jump;
        }
        else{
            time = 0;
        }
        if(PlayerStateManager.Instance.PlayerState == EPlayerState.Air && !_landHit.transform.IsUnityNull()) {
            _landHit = new();
        }

        PlayerStatus.MoveDirection = PlayerStatus.CurrentMoveDirection * Time.fixedDeltaTime;
        PlayerStatus.MoveDirection.x *= PlayerStatus.Speed;
        PlayerStatus.MoveDirection += PlayerStatus.CurrentAirDirection * PlayerStatus.FallGravity * time * time;
        PlayerStatus.NextPos = PlayerStatus.CurrentPos + PlayerStatus.MoveDirection;

        CheckCollide();

        _playerRigidBody2d.MovePosition(PlayerStatus.NextPos);   
    }


    private void CheckCollide(){
        Vector2 CastDirection = PlayerStatus.NextPos - PlayerStatus.CurrentPos;
        float CastDistance = Vector2.Distance(PlayerStatus.CurrentPos, PlayerStatus.NextPos);
        hitCount = _playerCollider2d.Cast(CastDirection, _hits, CastDistance);

        float d = 100;
        if(!_landHit.transform.IsUnityNull()) {
            Bounds bound = _landHit.collider.bounds;
            Vector2 a = bound.center + bound.size / 2;
            Vector2 b = PlayerStatus.NextPos - (Vector2)_playerCollider2d.bounds.size / 2;
            a.x = 0;
            b.x = 0;
            d = Vector2.Distance(a, b);
        }
        if (hitCount == 0 || !HitsPoisitonControl(_hits, d)){
            PlayerStateManager.Instance.PlayerState = EPlayerState.Air;
            PlayerStatus.isKoyote = true;
        } 
        
    }



    private bool HitsPoisitonControl(RaycastHit2D[] hits, float d){
        bool isLand = false;
        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D k = _hits[i];
            Bounds b = _playerCollider2d.bounds;
            float angle = Math.Abs(Vector2.Angle(Vector2.up, k.normal));

            if (k.normal == Vector2.left && _moveVector != Vector2.left)
            {
                PlayerStatus.NextPos.x = k.collider.ClosestPoint(PlayerStatus.CurrentPos).x - (b.size.x - 0.2f) / 2;
                PlayerStatus.CurrentMoveDirection.x = 0;
            }
            else if (k.normal == Vector2.right && _moveVector != Vector2.right)
            {
                PlayerStatus.NextPos.x = k.collider.ClosestPoint(PlayerStatus.CurrentPos).x + (b.size.x - 0.2f) / 2;
                PlayerStatus.CurrentMoveDirection.x = 0;
            }

            else if ((angle<45 && d < 0.1f) || (angle<45 && _landHit.transform.IsUnityNull()))
            {
                PlayerStateManager.Instance.PlayerState = EPlayerState.Idle;
                _landHit = k;
                isLand = true;
                PlayerStatus.NextPos.y = k.collider.ClosestPoint(PlayerStatus.CurrentPos).y + (b.size.y-0.2f) / 2;
                PlayerStatus.CurrentMoveDirection.y = 0;
                PlayerStatus.CurrentAirDirection = Vector2.zero;
            }

            else if(k.normal == Vector2.down && k.transform.tag != "SemiTerrain"){
                PlayerStatus.CurrentMoveDirection.y = 0;
            }


            print(Vector2.Angle(Vector2.up, k.normal) +" "+ k.normal);
            if(k.normal != Vector2.up) Debug.DrawLine(k.point, k.point+k.normal,Color.red,5);
        }
        return isLand;
    }


    protected override void SetInstance() => Instance = this;
}