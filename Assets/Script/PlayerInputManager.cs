

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
    private PlayerInput _playerInput;
    public int hitCount;
    
    public float time;

    void Start(){
        _playerRigidBody2d = GetComponent<Rigidbody2D>();
        _playerCollider2d = GetComponent<CapsuleCollider2D>();
        _playerInput = GetComponent<PlayerInput>();
        _hits = new RaycastHit2D[10];
    }

    public void FixedUpdate(){
        MoveFixedUpdate();
    }

    public void OnJump(InputAction.CallbackContext callback){
        if(callback.started && 
        (PlayerStateManager.Instance.PlayerConditionState != EPlayerConditionState.Air 
        || PlayerStateManager.Instance.PlayerConditionState == EPlayerConditionState.Koyote)){
            time= 0;
            PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Jump;
        }
    }

    public void OnMove(InputAction.CallbackContext callback){
        Vector2 inputVector = callback.ReadValue<Vector2>();
        PlayerStatus.CurrentInputDirection = inputVector;
        if (callback.canceled) PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Stop;
        
        else if (PlayerStateManager.Instance.PlayerConditionState == EPlayerConditionState.Air 
        || PlayerStateManager.Instance.PlayerConditionState == EPlayerConditionState.Koyote
        || PlayerStateManager.Instance.PlayerConditionState == EPlayerConditionState.Idle) {
            PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Run;
        }
    }

    public void OnDash(InputAction.CallbackContext callback){
        //print(callback.started +" "+callback.performed +" "+callback.canceled);
    }

    public void OnRopeMove(InputAction.CallbackContext callback){
        print("??!");
    }

    
    public void MoveFixedUpdate(){;
        PlayerStatus.CurrentPos = _playerRigidBody2d.position;
        PlayerStatus.MoveDirection = ((PlayerStatus.CurrentMoveDirection + PlayerStatus.CurrentJumpDirection) * Time.fixedDeltaTime) + PlayerStatus.CurrentAirDirection * PlayerStatus.FallGravity * time * time;
        PlayerStatus.MoveDirection.x *= PlayerStatus.Speed;
        PlayerStatus.NextPos = PlayerStatus.CurrentPos + PlayerStatus.MoveDirection;
        CheckCollide();

        _playerRigidBody2d.MovePosition(PlayerStatus.NextPos);
    }

    public void SetInputMap(int flag){
        _playerInput.currentActionMap = _playerInput.actions.FindActionMap("RopeLocomotion");
    }


    //Player Collide
    private void CheckCollide(){
        Vector2 CastDirection = PlayerStatus.NextPos - PlayerStatus.CurrentPos;
        float CastDistance = Vector2.Distance(PlayerStatus.CurrentPos, PlayerStatus.NextPos);
        hitCount = _playerCollider2d.Cast(CastDirection, _hits, CastDistance);

        //Debug.DrawLine(PlayerStatus.NextPos , PlayerStatus.CurrentPos, Color.red, 5f);
        if (hitCount == 0 || !HitsPoisitonControl(_hits)){
           if (PlayerStateManager.Instance.PlayerInputState != EPlayerInputState.Jump && PlayerStateManager.Instance.PlayerConditionState != EPlayerConditionState.Air) 
           PlayerStateManager.Instance.PlayerConditionState = EPlayerConditionState.Koyote;
        else  PlayerStateManager.Instance.PlayerConditionState = EPlayerConditionState.Air;
        }

    }

    private bool HitsPoisitonControl(RaycastHit2D[] hits){
        bool isLand = false;
        bool isLR = false;
        bool isUD = false;
        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D k = _hits[i];
            Bounds b = _playerCollider2d.bounds;
            float angle = Math.Abs(Vector2.Angle(Vector2.up, k.normal));

            if (k.normal == Vector2.left  && !isLR){
                wallCollide(k, b, true);
                isLR = true;
            }
            else if (k.normal == Vector2.right  && !isLR){
                wallCollide(k, b, false);
                isLR = true;
            }
            else if (k.normal == Vector2.down && k.transform.tag != "SemiTerrain" && !isUD)
            {
                PlayerStatus.CurrentJumpDirection.y = 0;
                ChangeAirStateFinish();
            }
            else if (angle<45 && !isUD)
            {
                Vector2 closestPoint = k.collider.ClosestPoint(PlayerStatus.CurrentPos);
                Vector2 nextPoint = k.collider.ClosestPoint(PlayerStatus.NextPos);
               // print(PlayerStatus.MoveDirection.y);

                //SemiTerrain에 아래에서 위로 올라오는 경우 점프가 강제 종료되는 문제 걸러내는 조건문
                if (PlayerStatus.MoveDirection.y >= 0 && PlayerStateManager.Instance.PlayerInputState == EPlayerInputState.Jump) {
                    continue;
                }
                //일반적인 착지 동작
                else if (Vector2.Distance(PlayerStatus.NextPos, closestPoint) < 1.2f){
                    PlayerStateManager.Instance.PlayerConditionState = EPlayerConditionState.Idle;
                    ChangeAirStateFinish();
                

                    //print("d: " + Vector2.Distance(PlayerStatus.NextPos,closestPoint));
                    PlayerStatus.NextPos.y = k.collider.ClosestPoint(PlayerStatus.CurrentPos).y + (b.size.y-0.2f) / 2;
                    PlayerStatus.CurrentJumpDirection.y = 0;
                    PlayerStatus.CurrentAirDirection = Vector2.zero;
                    isLand = true;
                }
            }

        }
        return isLand;
    }

    private void wallCollide(RaycastHit2D hit, Bounds b, bool isLeft){
        Vector2 closestPoint = hit.collider.ClosestPoint(PlayerStatus.CurrentPos);
        Vector2 nextPoint = hit.collider.ClosestPoint(PlayerStatus.NextPos);
        //print(Vector2.Distance(nextPoint, closestPoint) + " " + Vector2.Distance(nextPoint, PlayerStatus.NextPos));

        if (Vector2.Distance(nextPoint, PlayerStatus.NextPos) < 0.5f && isLeft) PlayerStatus.NextPos.x = closestPoint.x - (b.size.x - 0.2f) / 2;
        else if (Vector2.Distance(nextPoint, PlayerStatus.NextPos) < 0.5f && !isLeft)   PlayerStatus.NextPos.x = closestPoint.x + (b.size.x - 0.2f) / 2;
        PlayerStatus.CurrentMoveDirection = Vector2.zero;
    }


    //공중에 뜬 상태(점프나 그냥 낙하)에서 지면에 닿거나 특정 물체에 충돌했을 경우 input state를 현재 inputvector에 따라서 바꿔주는 함수
    //코드 중복 방지하려고 만든 함수고 크리티컬한 부분은 아니임
    private void ChangeAirStateFinish(){
        if (PlayerStatus.CurrentInputDirection == Vector2.zero) PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Stop;
        else PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Run;
    }


    protected override void SetInstance() => Instance = this;
}