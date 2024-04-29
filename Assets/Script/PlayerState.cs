using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : IConditionState
{
    public void PlayConditionState(Transform transform, Rigidbody2D rigidbody2D){
        PlayerInputManager.Instance.time = 0;
    }
}


public class AirState : IConditionState
{
    public void PlayConditionState(Transform transform, Rigidbody2D rigidbody2D)
    {
        PlayerInputManager.Instance.time += Time.fixedDeltaTime;
        PlayerStatus.CurrentAirDirection = Vector2.down;
    }
}

public class KoyoteState : IConditionState{
    private float _koyoteTime;

    public void PlayConditionState(Transform transform, Rigidbody2D rigidbody2D)
    {
        PlayerInputManager.Instance.time += Time.fixedDeltaTime;
        PlayerStatus.CurrentAirDirection = Vector2.down;
        if (_koyoteTime >= PlayerStatus.KoyoteTime){
            _koyoteTime = 0;
            PlayerStateManager.Instance.PlayerConditionState = EPlayerConditionState.Air;
        }
        else _koyoteTime += Time.fixedDeltaTime;
        
    }
}

public class DeadState : IConditionState
{
    public void PlayConditionState(Transform transform, Rigidbody2D rigidbody2D)
    {

    }
}




public class MovingState{
    protected void Run(){
            PlayerStatus.CurrentMoveDirection += PlayerStatus.CurrentInputDirection/10;
            if(PlayerStatus.CurrentMoveDirection.magnitude > PlayerStatus.CurrentInputDirection.magnitude){
                PlayerStatus.CurrentMoveDirection = PlayerStatus.CurrentInputDirection;
        }
    }

    protected void Stop(){
            Vector2 accelDirection = Vector2.zero - PlayerStatus.CurrentMoveDirection;
            PlayerStatus.CurrentMoveDirection += accelDirection / 5;
            if (PlayerStatus.CurrentMoveDirection.magnitude < 0.1f) PlayerStatus.CurrentMoveDirection = Vector2.zero;
        
    }

    protected void Jump(){
        PlayerStatus.CurrentAirDirection = Vector2.down;
        PlayerStatus.CurrentJumpDirection.y = PlayerStatus.Jump;
    }
}

public class StopState : MovingState, IInputState
{
    public void PlayInputState(InputAction.CallbackContext context, Transform transform) {
        Stop();
    }
}



public class RunState : MovingState, IInputState
{
    public void PlayInputState(InputAction.CallbackContext context, Transform transform){
        Run();
    }
}

public class JumpState : MovingState, IInputState
{
    public void PlayInputState(InputAction.CallbackContext context, Transform transform)
    {
        Run();
        Jump();
    }
}
