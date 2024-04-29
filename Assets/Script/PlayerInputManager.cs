

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
    private Vector2 _landVector;
    public int hitCount;
    
    public float time;

    void Start(){
        _playerRigidBody2d = GetComponent<Rigidbody2D>();
        _playerCollider2d = GetComponent<CapsuleCollider2D>();
        _hits = new RaycastHit2D[10];
    }

    public void FixedUpdate(){
        _landVector = Vector2.Perpendicular(_landHit.normal);
        MoveFixedUpdate();
    }

    public void OnJump(InputAction.CallbackContext callback){
        if(callback.started && 
        (PlayerStateManager.Instance.PlayerConditionState != EPlayerConditionState.Air || PlayerStateManager.Instance.PlayerConditionState == EPlayerConditionState.Koyote)){
            PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Jump;
        }
    }

    public void OnMove(InputAction.CallbackContext callback){
        Vector2 inputVector = callback.ReadValue<Vector2>();
        PlayerStatus.CurrentInputDirection = inputVector;
        if (callback.canceled)
        {
            PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Stop;
        }
        else if (PlayerStateManager.Instance.PlayerConditionState == EPlayerConditionState.Air 
        || PlayerStateManager.Instance.PlayerConditionState == EPlayerConditionState.Koyote
        || PlayerStateManager.Instance.PlayerConditionState == EPlayerConditionState.Idle) {
            PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Run;
        }
    }

    public void MoveFixedUpdate(){;
        //player run 상태 변경 요소
        PlayerStatus.CurrentPos = _playerRigidBody2d.position;
        if(PlayerStateManager.Instance.PlayerConditionState == EPlayerConditionState.Air && !_landHit.transform.IsUnityNull()) 
        _landHit = new();
        
        PlayerStatus.MoveDirection = ((PlayerStatus.CurrentMoveDirection + PlayerStatus.CurrentJumpDirection) * Time.fixedDeltaTime) + PlayerStatus.CurrentAirDirection * PlayerStatus.FallGravity * time * time;
        PlayerStatus.MoveDirection.x *= PlayerStatus.Speed;
        PlayerStatus.NextPos = PlayerStatus.CurrentPos + PlayerStatus.MoveDirection;
        CheckCollide();
        _playerRigidBody2d.MovePosition(PlayerStatus.NextPos);
    }


    private void CheckCollide(){
        Vector2 CastDirection = PlayerStatus.NextPos - PlayerStatus.CurrentPos;
        float CastDistance = Vector2.Distance(PlayerStatus.CurrentPos, PlayerStatus.NextPos);
        hitCount = _playerCollider2d.Cast(CastDirection, _hits, CastDistance);
        Debug.DrawLine(PlayerStatus.NextPos , PlayerStatus.CurrentPos, Color.red, 5f);

        float d = 100;
        if (hitCount == 0 || !HitsPoisitonControl(_hits, d)){
           // if (PlayerStateManager.Instance.PlayerConditionState != EPlayerConditionState.Air)
               // PlayerStateManager.Instance.PlayerConditionState = EPlayerConditionState.Koyote;
            PlayerStateManager.Instance.PlayerConditionState = EPlayerConditionState.Air;
        }

    }

    private bool HitsPoisitonControl(RaycastHit2D[] hits, float d){
        bool isLand = false;
        bool isLR = false;
        bool isUD = false;
        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D k = _hits[i];
            Bounds b = _playerCollider2d.bounds;
            float angle = Math.Abs(Vector2.Angle(Vector2.up, k.normal));

            if (k.normal == Vector2.left  && !isLR)
            {
                wallCollide(k, b, true);
                isLR = true;
            }
            else if (k.normal == Vector2.right  && !isLR)
            {
                wallCollide(k, b, false);
                isLR = true;
            }
            else if (k.normal == Vector2.down && k.transform.tag != "SemiTerrain" && !isUD)
            {
                PlayerStatus.CurrentJumpDirection.y = 0;
                if (PlayerStatus.CurrentInputDirection == Vector2.zero) PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Stop;
                else PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Run;
            }
            else if ((angle<45)&& !isUD)
            {
                Vector2 closestPoint = k.collider.ClosestPoint(PlayerStatus.CurrentPos);
                Vector2 nextPoint = k.collider.ClosestPoint(PlayerStatus.NextPos);
                print(Vector2.Distance(nextPoint, closestPoint) + " " + Vector2.Distance(nextPoint, PlayerStatus.NextPos));
                if (Vector2.Distance(PlayerStatus.NextPos, closestPoint) < 1.2f){

                    PlayerStateManager.Instance.PlayerConditionState = EPlayerConditionState.Idle;

                    if (PlayerStatus.CurrentInputDirection == Vector2.zero) PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Stop;
                    else PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Run;
                    
                    _landHit = k;
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

    private void wallCollide(RaycastHit2D k, Bounds b, bool isLeft){
        Vector2 closestPoint = k.collider.ClosestPoint(PlayerStatus.CurrentPos);
        Vector2 nextPoint = k.collider.ClosestPoint(PlayerStatus.NextPos);
        //print(Vector2.Distance(nextPoint, closestPoint) + " " + Vector2.Distance(nextPoint, PlayerStatus.NextPos));

        if (Vector2.Distance(nextPoint, PlayerStatus.NextPos) < 0.5f && isLeft) PlayerStatus.NextPos.x = closestPoint.x - (b.size.x - 0.2f) / 2;
        else if (Vector2.Distance(nextPoint, PlayerStatus.NextPos) < 0.5f && !isLeft)   PlayerStatus.NextPos.x = closestPoint.x + (b.size.x - 0.2f) / 2;
        PlayerStatus.CurrentMoveDirection = Vector2.zero;
    }


    protected override void SetInstance() => Instance = this;
}
















public static class PlayerPhysicsCollidingSystem{
    static RaycastHit2D[] hits = new RaycastHit2D[10];

    public static void CheckCollide(Collider2D playerCollider2d, RaycastHit2D landHit, ref float koyoteTime)
    {
        Vector2 CastDirection = PlayerStatus.NextPos - PlayerStatus.CurrentPos;
        float CastDistance = Vector2.Distance(PlayerStatus.CurrentPos, PlayerStatus.NextPos);
        int hitCount = playerCollider2d.Cast(CastDirection, hits, CastDistance);

        float d = 100;
        if (!landHit.transform.IsUnityNull())
        {
            Bounds bound = landHit.collider.bounds;
            Vector2 a = bound.center + bound.size / 2;
            Vector2 b = PlayerStatus.NextPos - (Vector2)playerCollider2d.bounds.size / 2;
            a.x = 0;
            b.x = 0;
            d = Vector2.Distance(a, b);
        }
        if (hitCount == 0 || !HitsPoisitonControl(playerCollider2d, landHit, hitCount, d, ref koyoteTime)){
            if(PlayerStateManager.Instance.PlayerConditionState != EPlayerConditionState.Air) 
                PlayerStateManager.Instance.PlayerConditionState = EPlayerConditionState.Koyote;
        }
    }

    public static bool HitsPoisitonControl(Collider2D playerCollider2d, RaycastHit2D landHit,int hitCount, float d, ref float koyoteTime)
    {
        bool isLand = false;
        bool isLR = false;
        bool isUD = false;

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D k = hits[i];
            Bounds b = playerCollider2d.bounds;
            float angle = Math.Abs(Vector2.Angle(Vector2.up, k.normal));

            if (k.normal == Vector2.left && PlayerStatus.CurrentInputDirection != Vector2.left && !isLR)
            {
                PlayerStatus.NextPos.x = k.collider.ClosestPoint(PlayerStatus.CurrentPos).x - (b.size.x - 0.2f) / 2;
                PlayerStatus.CurrentMoveDirection.x = 0;
                isLR = true;
            }
            else if (k.normal == Vector2.right && PlayerStatus.CurrentInputDirection != Vector2.right && !isLR)
            {
                PlayerStatus.NextPos.x = k.collider.ClosestPoint(PlayerStatus.CurrentPos).x + (b.size.x - 0.2f) / 2;
                PlayerStatus.CurrentMoveDirection.x = 0;
                isLR = true;
            }
            else if ((angle < 45 && d < 0.1f) || (angle < 45 && landHit.transform.IsUnityNull()) && !isUD)
            {
                PlayerStateManager.Instance.PlayerConditionState = EPlayerConditionState.Idle;
                PlayerStateManager.Instance.PlayerInputState = EPlayerInputState.Stop;
                landHit = k;
                isLand = true;
                isUD = true;
                koyoteTime = 0;
                PlayerStatus.NextPos.y = k.collider.ClosestPoint(PlayerStatus.CurrentPos).y + (b.size.y - 0.2f) / 2;
                PlayerStatus.CurrentMoveDirection.y = 0;
                PlayerStatus.CurrentAirDirection = Vector2.zero;
            }
            else if (k.normal == Vector2.down && k.transform.tag != "SemiTerrain"){
                PlayerStatus.CurrentMoveDirection.y = 0;
                isUD = true;
            }

            if(isUD && isLR) break;
            if (k.normal != Vector2.up) Debug.DrawLine(k.point, k.point + k.normal, Color.red, 5);
        }

        return isLand;
    }
}
